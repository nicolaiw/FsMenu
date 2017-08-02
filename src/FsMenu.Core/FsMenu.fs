
module FsMenu.Core 

    type AfterExecution =
     | NavigateBack
     | Stay
     | Exit
        
    type MenuEntry =
    | Action of (unit -> AfterExecution)
    | Sub of (string * MenuEntry) list

    open System
    open FsMenu.Colors

    (* Small DSL *)
    let Menu = Sub
    
    // Render sub menu
    let (+>) name entry : (string * MenuEntry) = (name,entry)
    
    // Execute action and exit
    let (=>) s f = (s, Action (fun () -> f(); Exit))
    
    // Execute action and render the previous menu
    let (<+) s f = (s, Action (fun () -> f(); NavigateBack))
    
    // Execute action and render the menu where you come frome
    let (<+=) s f = (s, Action (fun () -> f(); Stay))
    
    
    // TODO: 
    //       do some reorg
    //       use Fake (F# Make)
    //       create Nuget
    //       write some "readme"-stuff
    
    
    // TODO: sepearte functions for rendering an emphaziser and an entry
    // Call both to colorize the line

    type private Colorize =
    | Emphasizer
    | Entry
    | Both

    let colorizeEmphasizer (oldIndex: int) (newIndex:int) (emphasizer: string) (color: Color) (entries: string list) =
        () // TODO: Implement

    let colorizeEntry (oldIndex: int) (newIndex:int) (color: Color) (entries: string list) =
        () // TODO: Implement

    let colorizeLine (oldIndex: int) (newIndex:int) (emphasizer: string) (color: Color) (entries: string list) =
        () // TODO: Implement

    (* Emphasizes an entry *)
    let private emphasizeEntry (oldIndex: int) (newIndex:int) (emphasizer: string) (color: Color) (entries: string list) =

        // TODO: Do somehtin with coloirze
        // macht colorize with
        // | Emphaziser -> colorizeEmphaziser ...
        // | Entry -> ...
        // .
        // .
        // .

        let inititalCursorPos = Console.CursorTop
    
        (* Clear the current emphasizer *)
        match emphasizer with
        | e when e = String.Empty ->
            Console.SetCursorPosition(0, Console.CursorTop - (entries.Length-oldIndex))
            Console.Write(entries.[oldIndex])
        | _ ->
            Console.SetCursorPosition(entries.[oldIndex].Length, Console.CursorTop - (entries.Length-oldIndex))
            Console.Write(emphasizer.[oldIndex])
        
        (* Emphasise the new entry *)
        Console.SetCursorPosition(0, inititalCursorPos)
        
        let currentForegroundColor = Console.ForegroundColor

        match color with
        | Color.None ->
                Console.Write(sprintf " %s" emphasizer)
        
        | c as consoleColor ->

             
             Console.ForegroundColor <- enum<ConsoleColor> (unbox<int32> c)
             
             match emphasizer with
             | e when e = String.Empty ->
                Console.SetCursorPosition(0, Console.CursorTop - (entries.Length-newIndex))
                Console.Write(sprintf "%s" entries.[newIndex])
             | _ ->
                Console.SetCursorPosition(entries.[newIndex].Length, Console.CursorTop - (entries.Length-newIndex))
                Console.Write(sprintf " %s" emphasizer)
                
        Console.ForegroundColor <- currentForegroundColor
       
        (* Reset the Cursor *)
        Console.SetCursorPosition(0, inititalCursorPos);
    
    
    (* Clears the current menu *)
    let private clear entryCount =
        let diff = Math.Abs (Console.CursorTop - entryCount)
        
        for i=0 to entryCount do
            Console.SetCursorPosition(0, i + diff);
            Console.Write(new string(' ', Console.WindowWidth))
    
        Console.SetCursorPosition(0, diff);
    
    
    (* Shows the menu and handles Input *)
    let private renderColored menuEntry emphesizer color =
    
        (* All recursive stuff is hidden for the user of "render" *)
        let rec renderSubMenuRec callStack menuEntry emphasize = 
            match menuEntry with
            | Sub (subMenu) -> 
                for i = 0 to subMenu.Length-1 do
                     let name = fst subMenu.[i]
                     //let entry = if i = emphasize then sprintf "%s %s" name emphesizer else name
                     printfn "%s" name;
                
                subMenu
                |> List.map fst
                |> emphasizeEntry 0 0 emphesizer color 

                let rec handleUserInput currentEntry = 
                    let cki = Console.ReadKey(true)
    
                    match cki.Key with
                    | ConsoleKey.DownArrow -> 
                        if currentEntry+1 < subMenu.Length then
                           subMenu |> List.map fst |> emphasizeEntry currentEntry (currentEntry+1) emphesizer color
                           handleUserInput (currentEntry+1)
                        else
                           handleUserInput currentEntry
    
                    | ConsoleKey.UpArrow -> 
                        if currentEntry-1 >= 0 then
                           subMenu |> List.map fst |> emphasizeEntry currentEntry (currentEntry-1) emphesizer color
                           handleUserInput (currentEntry-1)
                        else
                           handleUserInput currentEntry
    
                    | ConsoleKey.Enter ->
                        (* Either call the action or navigate downwards *)
                        match snd subMenu.[currentEntry] with
                        | Action handler ->
                            let cursorPosBeforeExecute = Console.CursorTop
                            
                            match handler() with
                            | NavigateBack ->
                                (* If the user does some prints we have to delete those lines as well *)
                                let cursorDiff = Console.CursorTop-cursorPosBeforeExecute
                                clear (subMenu.Length+cursorDiff)
    
                                let (previousEntry, stack) =
                                    match callStack with
                                    | [] -> (menuEntry, callStack)
                                    | hd::tail -> (hd,tail)
                                
                                renderSubMenuRec stack previousEntry emphasize
    
                            | Stay ->
                                (* If the user does some prints we have to delete those lines as well *)
                                let cursorDiff = Console.CursorTop-cursorPosBeforeExecute
                                clear (subMenu.Length+cursorDiff)
                                renderSubMenuRec callStack menuEntry emphasize
    
                            | Exit -> ()
                        
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
    

    let render menuEntry emphesizer =
        renderColored menuEntry emphesizer Color.None

    let renderWithColoredEmphaziser menuEntry emphaziser color =
        renderColored menuEntry emphaziser color

    let renderWithColoredEntry menuEntry color =
        renderColored menuEntry System.String.Empty color
