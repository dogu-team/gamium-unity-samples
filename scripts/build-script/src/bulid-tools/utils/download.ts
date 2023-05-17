import fs from 'fs';
import fspromise from 'fs/promises';
import path from 'path';
import { HttpClient } from 'typed-rest-client/HttpClient';

interface Headers {
  [key: string]: string | number;
}

export async function download(url: string, destPath: string, headers: Headers): Promise<boolean> {
  const httpc = new HttpClient('');
  const destDirectoryPath = path.dirname(destPath);
  await fspromise.mkdir(destDirectoryPath, { recursive: true });

  const file: NodeJS.WritableStream = fs.createWriteStream(destPath);
  const res = await httpc.get(url, headers);
  return new Promise((resolve, reject) => {
    const stream = res.message.pipe(file);
    console.info(`download. ${url}`);
    stream
      .on('error', (error) => {
        console.error(error);
        reject(error);
      })
      .on('data', (_) => {
        return;
      })
      .on('finish', (_) => {
        const isOk = res.message.statusCode === 200;
        const destStat = fs.statSync(destPath);
        const contentLength: number = res.message.headers['content-length'] ? Number(res.message.headers['content-length']) : 0;
        const isSizeEqual = contentLength === destStat.size;
        if (!isOk) {
          console.error(`download failed. url:${url}, dest:${destPath}, code:${res.message.statusCode ?? 'undefined'}, contentLength:${contentLength}, statSize:${destStat.size}`);
          reject(res.message.statusCode);
          return;
        }
        if (0 < contentLength && !isSizeEqual) {
          console.error(`download failed. url:${url}, dest:${destPath}, code:${res.message.statusCode ?? 'undefined'}, contentLength:${contentLength}, statSize:${destStat.size}`);
          reject(res.message.statusCode);
          return;
        }
        resolve(true);
      });
  });
}
