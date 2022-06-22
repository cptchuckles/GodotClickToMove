# Godot Click-to-Move project example

This project was created to showcase a decoupled way to implement click-to-move in a potentially multiplayer game.  The project includes a stub for a "client" and "server", who exchange signals with each other in order to govern the instantiation and movement of Character objects in the game world.

Click-to-move is implemented as such:
 - `ClickInputHandler` detects click events in its `_UnhandledInput` callback.  Upon a click, it directs the `WebsocketClient` singleton to emit a request for movement.
 - `WebsocketClient` emits a request signal to move to the given coordinates, forwarding its `_clientId` field.  This signal is received by the `RemoteServerStub`, which processes the request to move.
 - `RemoteServerStub` will approve the movement request, and emit a `MovementEvent` signal, forwarding the `clientId` who requested to move, and its desired `destination`.
 - `WebsocketClient` receives a `MovementEvent` signal from the `RemoteServerStub`, and finds the `Character` who matches the given `clientId`.  It then sets that `Character`'s `Destination` property to the given coordinates.
 - `Character` instances are always trying to move towards their `Destination`.  By default, their `Destination` is set to their spawn position, such that they don't have anywhere to go.  The `WebsocketClient` is responsible for telling all `Character` instances where their respsective `Destionation`s are, governing the motion of all players in the client program.
