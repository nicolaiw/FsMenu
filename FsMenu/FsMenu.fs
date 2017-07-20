module Menu

open System


type MenuEntry =
| Action of (unit -> unit)
| Sub of (string * MenuEntry) list

(* Small DSL *)
let Menu = Sub

let (+>) name entry : (string * MenuEntry) = (name,entry)

let (=>) s f = (s, Action f)

// TODO: make the helpers private

(* Emphasizes an entry *)
let emphasizeEntry (oldIndex: int) (newIndex:int) (emphasizer: string) (entries: string list) =

    let inititalCursorPos = Console.CursorTop

    (* Clear the current emphasizer *)
    Console.SetCursorPosition(entries.[oldIndex].Length, Console.CursorTop - (entries.Length-oldIndex))
    Console.Write(new string(' ', emphasizer.Length + 1))
    (* Emphasise the new entry *)
    Console.SetCursorPosition(0, inititalCursorPos)
    Console.SetCursorPosition(entries.[newIndex].Length, Console.CursorTop - (entries.Length-newIndex))
    Console.Write(sprintf " %s" emphasizer)
    (* Reset the Cursor *)
    Console.SetCursorPosition(0, inititalCursorPos);


(* Clears the current menu *)
let clear entryCount =
    let diff = Math.Abs (Console.CursorTop - entryCount)
    
    for i=0 to entryCount do
        Console.SetCursorPosition(0, i + diff);
        Console.Write(new string(' ', Console.WindowWidth))

    Console.SetCursorPosition(0, diff);

(* Shows the menu and handles Input *)
let render menuEntry emphesizer =

    (* All recursive stuff is hidden for the user of "render" *)
    let rec renderSubMenuRec callStack menuEntry emphasize = 
        match menuEntry with
        | Sub (subMenu) -> 
            for i = 0 to subMenu.Length-1 do
                 let name = fst subMenu.[i]
                 let entry = if i = emphasize then sprintf "%s %s" name emphesizer else name
                 printfn "%s" entry;
            
            let rec handleUserInput currentEntry = 
                let cki = Console.ReadKey(true)

                match cki.Key with
                | ConsoleKey.DownArrow -> 
                    if currentEntry+1 < subMenu.Length then
                       subMenu |> List.map fst |> emphasizeEntry currentEntry (currentEntry+1) emphesizer
                       handleUserInput (currentEntry+1)
                    else
                       handleUserInput currentEntry

                | ConsoleKey.UpArrow -> 
                    if currentEntry-1 >= 0 then
                       subMenu |> List.map fst |> emphasizeEntry currentEntry (currentEntry-1) emphesizer
                       handleUserInput (currentEntry-1)
                    else
                       handleUserInput currentEntry

                | ConsoleKey.Enter ->
                    (* Either call the action or navigate downwards *)
                    match snd subMenu.[currentEntry] with
                    | Action handler ->
                        handler()
                    | Sub sub ->
                        clear subMenu.Length
                        renderSubMenuRec (menuEntry::callStack) (snd subMenu.[currentEntry]) 0

                | ConsoleKey.Backspace ->
                    match callStack with
                    | [] ->
                        handleUserInput currentEntry

                    | hd::t -> // hd contains the previous
                        clear subMenu.Length
                        renderSubMenuRec t hd 0

                | _ -> handleUserInput currentEntry
                                                     
            handleUserInput emphasize  
        | _ -> ()

    renderSubMenuRec [] menuEntry 0
