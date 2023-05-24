from typing import Awaitable, Callable


async def test(name: str, func: Callable[[], Awaitable[None]]):
    print(f"Running {name}...")
    await func()
