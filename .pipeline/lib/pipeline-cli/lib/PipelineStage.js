//
// pipeline-cli
//
// Copyright © 2019 Province of British Columbia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* eslint-disable */
'use strict';
const isFunction = require('lodash.isfunction');

module.exports = class Stage {
  /**
   * {name, description, callback, stages, options}
   * @param {*} name 
   * @param {*} options 
   * @param {*} callback 
   */
  constructor(name, options=undefined, callback=undefined){
    if (isFunction(options) || options instanceof Array) {
      callback=options
      options=undefined
    }
  
    if (name == null) throw Error("name cannot be null or undefined")
    if (name.indexOf('.')>=0) throw Error("name cannot contain dot(.)")
    this.name=name;
    this.steps=callback;
    this.options=options || {}
    this._path = name
    this.skip=false

    //console.log(`creating '${name}'`)
    if (this.steps instanceof Array){
      for (var i=0; i < this.steps.length; i++){
        var step= this.steps[i]
        step._parent=this
      }
    }
  }
  static create(){
    var args = Array.prototype.slice.call(arguments, 0)
    args.unshift(null)
    var clazz = Stage;
    var object = new (Function.prototype.bind.apply(clazz, args))
  
    //constructor.apply(object, args);
    object._root=object
    
    return object; 
  }
  then (){
    var args = Array.prototype.slice.call(arguments, 0)
    args.unshift(null)
    var clazz = Stage;
    var object = new (Function.prototype.bind.apply(clazz, args))
  
    //constructor.apply(object, args);
    if(this !== global && this != null){
      object._root=this._root
      object._previous = this
      this._next = object
    }else{
      object._root=object
    }
    return object;
  }

  isGate(){
    return false;
  }
} // end stage