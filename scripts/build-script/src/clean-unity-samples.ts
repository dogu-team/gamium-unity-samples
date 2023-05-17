import path from 'path';
import shelljs from 'shelljs';
import { env } from './env';

const projectPath = path.resolve(env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLES_PATH, env.DOGU_GAMIUM_ENGINE_UNITY_SAMPLE_NAME);
if (shelljs.rm('-rf', path.resolve(projectPath, 'build')).code !== 0) {
  throw new Error('Failed to clean build directory');
}
