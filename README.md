# delivery-simulator

Delivery simulator based on three apps:
* OrderEmitter reads JSON file and passes orders to Kitchen
* Kitchen manages orders - puts them into specific shelves and handles shelf overflow, courier pickup and deterrioration events. Lists shelves contents
* EventLogDisplay - displays event from the kitchen.

Requires Erlang and RabbitMQ to be installed:
* Erlang https://www.erlang.org/downloads (64-Bit binary file)
* RabbitMQ https://www.rabbitmq.com/install-windows.html#downloads (rabbitmq-server-3.8.4.exe)

For debugging in Visual Studio use Multiple startup (can be found under solution properties). 
For projects OrderEmitter, Kitchen, EventLogDisplay set "Start" action.\
For CLI build execute "msbuild" command in repository root via Developer Command Prompt for VS.\
For CLI startup from bin folder projects should be started in following order: EventLogDisplay.exe -> Kitchen.exe -> OrderEmitter.exe.

Unit tests: requires NUnit extension for VS to properly run tests in Test Explorer.
