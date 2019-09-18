# node-listener-collection

[![Build Status](https://travis-ci.org/pofider/node-listener-collection.png?branch=master)](https://travis-ci.org/pofider/node-listener-collection)

This library extends the standard event mechanism with ability to wait until all asynchronous events are processed.

> npm install listener-collection


## Callback based
```js
var ListenerCollection = require("listener-collection");

var liseners = new ListenerCollection();

listeners.add("listener1", function(param1, param2, next) {
   setTimeout(function() {
     console.log("listener");  
     next();
   }, 100);
});

listeners.add("listener2", function(param1, param2, next) {
   setTimeout(function() {
     console.log("listener2");  
     next();
   }, 200);
});

listeners.fire("A", "B", function() {
  console.log("both listeners are processed now");
});
```

## Promise based
```js
var ListenerCollection = require("listener-collection");

var liseners = new ListenerCollection();

listeners.add("a name", function(param1, param2) {
   console.log("listener catch");  
   return promise;   
});

listeners.fire("A", "B").then(function() {
  console.log("everything is done");
});
```

## Listener context

```js
listeners.add("a name", context, function() {
   //this runs bound to context
});
```

## Listener position

Listeners are executed one by one in order they were added.  If you want to add a listener to a particular position you can use `insert` and specify the condition when the listener should be invoked.

```js
listeners.add("listener1", function() { ... });
listeners.add("listener2", function() { ... });
listeners.insert({ after: "listener1" }, "listener3", function() { ... });
```
Alternatively you can also use `before` instead or together with `after`. 

## Removing listener

```js
listeners.add("test", function () { });
listeners.remove("test");
```


## Hooks


```js
listeners.post(function() {
  console.log("this runs after the listeners are invoked");
});

listeners.pre(function() {
  console.log("this runs before the listeners are invoked");
});

listeners.postFail(function(err) {
  console.log("this runs after the listeners are invoked when one of the listeners fails");
});
```

## License 
MIT
