import { unity } from './bulid-tools/index';
import { OctokitContext } from './bulid-tools/git/octokit-context';
import compressing from 'compressing';
import fsPromises from 'fs/promises';
import path from 'path';
import shelljs from 'shelljs';
import { env } from './env';

const TagName = `${env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME}-${env.DOGU_GAMIUM_ENGINE_VERSION}`;
const AppVersionNumber = /^\d+\.\d+\.\d+$/.test(env.DOGU_GAMIUM_ENGINE_VERSION) ? env.DOGU_GAMIUM_ENGINE_VERSION : '0.0.1';

const client = new OctokitContext(env.DOGU_GITHUB_TOKEN).client;
interface ReleaseInfo {
  id: number;
  upload_url: string;
}

function getCommitHash(): string {
  const cwd = shelljs.pwd();
  shelljs.cd(env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH);
  const hash = shelljs.exec('git rev-parse --short=40 HEAD').trim();
  shelljs.cd(cwd);
  return hash;
}

async function createRelease(commit: string): Promise<ReleaseInfo> {
  const preExistRelease = await client
    .request('GET /repos/{owner}/{repo}/releases/tags/{tag}', {
      owner: 'dogu-team',
      repo: 'gamium-unity-samples',
      tag: TagName,
    })
    .then((response) => {
      return response;
    })
    .catch((error: Error) => {
      return null;
    });
  if (preExistRelease) {
    return {
      id: preExistRelease.data.id,
      upload_url: preExistRelease.data.upload_url,
    };
  } else {
    const res = await client.request('POST /repos/{owner}/{repo}/releases', {
      owner: 'dogu-team',
      repo: 'gamium-unity-samples',
      tag_name: TagName,
      target_commitish: commit,
      name: TagName,
      body: '-',
      draft: false,
      prerelease: false,
      generate_release_notes: false,
    });
    return {
      id: res.data.id,
      upload_url: res.data.upload_url,
    };
  }
}

async function setReleaseAsset(release: ReleaseInfo, filePath: string): Promise<void> {
  const fileName = path.basename(filePath);
  // get asset
  const preExistAsset = await client.request('GET /repos/{owner}/{repo}/releases/{release_id}/assets', {
    owner: 'dogu-team',
    repo: 'gamium-unity-samples',
    release_id: release.id,
  });
  // delete asset
  for (const asset of preExistAsset.data) {
    if (asset.name === fileName) {
      await client.request('DELETE /repos/{owner}/{repo}/releases/assets/{asset_id}', {
        owner: 'dogu-team',
        repo: 'gamium-unity-samples',
        asset_id: asset.id,
      });
    }
  }
  // upload asset
  await client.request('POST {url}', {
    url: release.upload_url,
    headers: {
      'content-type': 'application/zip',
      'content-length': 0,
    },
    name: fileName,
    data: await fsPromises.readFile(filePath),
  });
}

(async (): Promise<void> => {
  const projectPath = path.resolve(env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH, env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME);
  await unity.build(projectPath, {
    buildTarget: env.DOGU_UNITY_BUILD_TARGET as unity.BuildTarget,
    appVersion: AppVersionNumber,
    deps: [{ key: 'com.dogu.gamium.engine.unity', value: `https://github.com/dogu-team/gamium.git?path=/engine/unity#${env.DOGU_GAMIUM_ENGINE_VERSION}` }],
  });

  if (process.env.GITHUB_ACTION) {
    console.log(`[INFO] upload release asset to github`);
    const release = await createRelease(getCommitHash());
    // remove unncessary files
    const deleteTargets = [path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '*_ButDontShipItWithYourGame')];
    if (env.DOGU_UNITY_BUILD_TARGET === unity.BuildTarget.iOS) {
      deleteTargets.push(
        ...[
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/Unity-iPhone.xcarchive'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/MainApp'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/Libraries'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/Classes'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/Data'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/DerivedData'),
          path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET, '**/LaunchScreen*'),
        ],
      );
    }

    shelljs.rm('-rf', deleteTargets);
    // zip file
    const zipPath = path.resolve(projectPath, 'build', `${env.DOGU_UNITY_BUILD_TARGET}.zip`);
    await compressing.zip.compressDir(path.resolve(projectPath, 'build', env.DOGU_UNITY_BUILD_TARGET), zipPath);
    // upload file
    await setReleaseAsset(release, zipPath);
  }
})().catch((error: Error) => {
  throw error;
});
