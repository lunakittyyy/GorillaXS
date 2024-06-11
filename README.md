# GorillaXS
Websocket-based XSOverlay notification library for Gorilla Tag.

## Usage
Reference the GorillaXS DLL in your project and use it:

``using GorillaXS;``

Then:

``Notifier.Notify("My title", "My description");``

Only the title and the description are required, however you can also define notification height, timeout, icon, and audio.
