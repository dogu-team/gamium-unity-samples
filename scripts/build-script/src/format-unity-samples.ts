import { unity } from './bulid-tools/index';
import glob from 'glob';
import path from 'path';
import { env } from './env';

(async (): Promise<void> => {
  const projecstPath = path.resolve(env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH);
  // glob ProjectSettings.asset
  const projectSettingsPaths = glob.sync(path.resolve(projecstPath, '*/ProjectSettings/ProjectSettings.asset'));
  for (const projectSettingsPath of projectSettingsPaths) {
    await unity.modifyProjectSetting(path.resolve(projectSettingsPath, '..', '..'), '0.0.1');
  }
})().catch((error: Error) => {
  throw error;
});
