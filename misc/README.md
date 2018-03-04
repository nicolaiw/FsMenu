```fsharp
open System.Diagnostics
open System
open FsMenu.Core

let paketPath = System.IO.Path.Combine("..", "paket.exe")

let paket args =  
    let p = new Process()
    p.StartInfo.FileName <- paketPath
    p.StartInfo.CreateNoWindow <- false
    p.StartInfo.Arguments <- args
    p.StartInfo.UseShellExecute <- false
    p.StartInfo.RedirectStandardOutput <- true
    p.Start() |> ignore
    
    while not p.StandardOutput.EndOfStream do
     printfn "%s" (p.StandardOutput.ReadLine())
    

let paketAddSubMenu =
        let mutable add = "add"
        let mutable name = ""
        let mutable force = ""
        let mutable verbose = ""
        // ...
       
        let yesNoMenu onYes onNo =
             Menu [
                "yes" <+ onYes
                "no " <+ onNo] 
        
        let setName() = printf "name: "
                        name <- Console.ReadLine()

        let setForceMenu =
            yesNoMenu (fun () -> force <- "--force") (fun () -> force <- "")

        let setVerboseMenu =
            yesNoMenu (fun () -> verbose <- "--verbose") (fun () -> verbose <- "")

        let runAddWithArgs() = 
            [name
             force
             verbose]
             |> List.filter (fun arg -> arg <> "")
             |> List.fold (fun acc arg -> sprintf "%s %s" acc arg) add
             |> printf "%s"


        let men =
                Menu [
                    "name   " <+= setName
                    "force  " +>  setForceMenu
                    "verbose" +>  setVerboseMenu
                    "run_add" =>  runAddWithArgs]
        men

[<EntryPoint>]
let main argv = 
    
    printfn ""

    let test =
        Menu [
            "install    " => (fun () -> paket "install" |> ignore)
            "update     " => (fun () -> paket "update" |> ignore)
            "restore    " => (fun () -> paket "restore" |> ignore)
            "add package" +> paketAddSubMenu ]

    render test "|> exec"


    Console.ReadLine() |> ignore
    0
```

![](https://github.com/nicolaiw/FsMenu/blob/master/misc/sample2.gif)