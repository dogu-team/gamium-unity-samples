export async function test(name: string, fn: () => Promise<void> | void): Promise<void> {
  console.log(`[${name}]`);
  await fn();
}
