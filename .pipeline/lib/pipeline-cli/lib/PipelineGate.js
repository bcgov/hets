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

const Stage = require('./PipelineStage');

module.exports = class Gate extends Stage {
  constructor(name, options=undefined, callback=undefined){
    super(name, options, callback)
  }

  static create(){
    var args = Array.prototype.slice.call(arguments, 0)
    args.unshift(null)
    var clazz = Gate;
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
    return true;
  }
} //end Gate
