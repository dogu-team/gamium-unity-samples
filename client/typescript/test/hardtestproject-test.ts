import assert from 'assert';
import { By, GamiumClient, KeyBy, NodeGamiumService, ObjectHierarchyNode, Player, RpcBy, UnityKeyCode } from 'gamium';
import { test } from './functions';

function recursiveCounting(node: ObjectHierarchyNode): number {
  let count = 1;
  for (const child of node.children) {
    count += recursiveCounting(child);
  }
  return count;
}

(async () => {
  const gamiumService = new NodeGamiumService('127.0.0.1', 50061);
  await gamiumService.connect();
  const gamium = new GamiumClient(gamiumService);
  const inspector = gamium.inspector();
  const ui = gamium.ui();

  await test('Dump test', async () => {
    const start = Date.now();
    const hierarchys = await inspector.dumpHierarchy('', 0);
    let count = 0;
    for (const h of hierarchys) {
      for (const c of h.children) {
        count += recursiveCounting(c);
      }
    }

    console.log('time', Date.now() - start);
    console.log('count', count);
  });
})().catch((e) => {
  console.error(e);
  process.exit(1);
});
