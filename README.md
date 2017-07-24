# FsMenu

> A very small DSL to create an interactive cli.

### Usage

#### The DSL:

```fsharp
// Creates a new menu
let Menu = Sub

// Render sub menu
let (+>) name entry : (string * MenuEntry) = (name,entry)

// Execute action and Exit
let (=>) s f = (s, Action f)

// Execute action and Navigate back to the menu where you come from
let (<+) s f = (s, Action (fun () -> f(); NavigateBack))
```

#### Example:

```fsharp

let testFunc() = 
    printfn "selected Sub Sub 3"
    printf "handle some input: "
    let input = Console.ReadLine()
    // Do some stuff with input
    ()

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
                               "Sub Sub 3" <+ testFunc]
                   "Sub 2" => (fun () -> printf "selected Sub 2") ]]

    render test "<--"
```

> Instead of `(fun () -> printf ...` you could pass any `unit -> unit` function.

### Will turn into

![](https://github.com/nicolaiw/FsMenu/blob/master/misc/sample.gif)