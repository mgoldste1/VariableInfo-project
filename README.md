# VInfoExample

This lets you forget about if things you are working on are properties or fields.

```
var vars = typeof(MyClass).GetAllVariables();
foreach(var v in vars)
{
  if(v.IsAttributeDefined(typeof(MyAttribute)))
    //do something, like set the variable equal to a default value in an attribute if the var value is null.
}
```
