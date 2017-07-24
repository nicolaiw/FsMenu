# FsMenu

> A very small DSL to create an interactive cli.

### Usage

```fsharp
printfn "Use Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

let test =
        Menu [
            "Item 1" => (fun () -> printf "selected Item 1")
            "Item 2" +>
               Menu [ 
                   "Sub 1" +>
                           Menu [		   // (unit -> unit)  
                               "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                               "Sub Sub 2" => (fun () -> printf "selected Sub Sub 2")
                               "Sub Sub 3" => (fun () -> printf "selected Sub Sub 3")]
                   "Sub 2" => (fun () -> printf "selected Sub 2") ]]

    render test "<--"
```

### Will turn into

![](https://github.com/nicolaiw/FsMenu/blob/master/misc/sample.gif)