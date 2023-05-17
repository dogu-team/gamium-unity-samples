export class Env {
  DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH: string = process.env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH ?? '';
  DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME: string = process.env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME ?? '';
  DOGU_GAMIUM_ENGINE_VERSION: string = process.env.DOGU_GAMIUM_ENGINE_VERSION ?? '';
  DOGU_UNITY_BUILD_TARGET: string = process.env.DOGU_UNITY_BUILD_TARGET ?? '';
  DOGU_GITHUB_TOKEN: string = process.env.DOGU_GITHUB_TOKEN ?? '';
}

export const env = new Env();
if (env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH === '') throw new Error('DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH is not set');
if (env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME === '') throw new Error('DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME is not set');
if (env.DOGU_GAMIUM_ENGINE_VERSION === '') throw new Error('DOGU_GAMIUM_ENGINE_VERSION is not set');
if (env.DOGU_UNITY_BUILD_TARGET === '') throw new Error('DOGU_UNITY_BUILD_TARGET is not set');
if (env.DOGU_GITHUB_TOKEN === '') {
  console.warn('DOGU_GITHUB_TOKEN is not set. upload will not be triggered');
}
