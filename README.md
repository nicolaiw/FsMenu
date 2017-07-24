# FsMenu

> A very small DSL to create an interactive cli.

### Usage

The Domain type is defined as follows:

```fsharp
type MenuEntry =
| Action of (unit -> unit)
| Sub of (string * MenuEntry) list
```

The DSL:

let Menu = Sub

// name:string -> entry:MenuEntry -> string * MenuEntry
let (+>) name entry : (string * MenuEntry) = (name,entry)

// s:'a -> f:(unit -> unit) -> 'a * MenuEntry
let (=>) s f = (s, Action f) 

# Example

```fsharp
printfn "Use Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

let test =
        Menu [
            "Item 1" => (fun () -> printf "selected Item 1")
            "Item 2" +>
               Menu [ 
                   "Sub 1" +>
                           Menu [
                               "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                               "Sub Sub 2" => (fun () -> printf "selected Sub Sub 2")
                               "Sub Sub 3" => (fun () -> printf "selected Sub Sub 3")]
                   "Sub 2" => (fun () -> printf "selected Sub 2") ]]

    render test "<--"
```

> Instead of `printf` you could 

### Will turn into

![](https://github.com/nicolaiw/FsMenu/blob/master/misc/sample.gif)