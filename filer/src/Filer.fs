module Filer

open System
open System.IO
open System.Text

// Função para definir a posição do cursor no console
let updateCursorPosition cursorPosition =
    
    Console.SetCursorPosition(cursorPosition % Console.WindowWidth, cursorPosition / Console.WindowWidth)
    
// Função para imprimir o conteúdo e atualizar a posição do cursor
let printContentAndUpdateCursor (content: string) cursorPosition =
    
    Console.Clear()
    Console.Write(content)
    
    updateCursorPosition cursorPosition

// Função recursiva para o loop de edição
let rec editLoop (sb: StringBuilder) cursorPosition =
    
    printContentAndUpdateCursor (sb.ToString()) cursorPosition
    
    let keyInfo = Console.ReadKey(intercept = true)
    
    match keyInfo.Key with
    
    // TODO: Só funciona para o texto, o cursor no terminal segue sem quebrar a linha, apenas da um espaço
    | ConsoleKey.Enter ->
        
        // Adiciona uma nova linha no conteúdo e atualiza a posição do cursor
        sb.Insert(cursorPosition, '\n') |> ignore
        let newCursorPosition = cursorPosition + 1
        
        // move o cursor para a próxima linha
        Console.SetCursorPosition(0, Console.CursorTop + 1)
        
        // Chama a função recursiva para continuar a edição
        editLoop sb newCursorPosition
        
    | ConsoleKey.Backspace when cursorPosition > 0 ->
        
        // Remove o caractere anterior e atualiza a posição do cursor
        let newCursorPosition = cursorPosition - 1
        sb.Remove(newCursorPosition, 1) |> ignore
        
        // Chama a função recursiva para continuar a edição
        editLoop sb newCursorPosition
        
    | ConsoleKey.Backspace ->
        
        editLoop sb cursorPosition // se estiver no inicio da linha, apenas continua a edição
        
    | ConsoleKey.LeftArrow when cursorPosition > 0 ->
        
        // move o cursor para a esquerda
        let newCursorPosition = cursorPosition - 1
        updateCursorPosition newCursorPosition
        
        editLoop sb newCursorPosition
        
    | ConsoleKey.RightArrow when cursorPosition < sb.Length ->
        let newCursorPosition = cursorPosition + 1
        updateCursorPosition newCursorPosition
        
        editLoop sb newCursorPosition
        
    | ConsoleKey.Escape ->
        
        sb.ToString() // Sai do loop de edição e retorna o conteúdo
        
    | _ ->
        
        // Adiciona o caractere digitado e atualiza a posição do cursor
        sb.Insert(cursorPosition, keyInfo.KeyChar) |> ignore
        let newCursorPosition = cursorPosition + 1
        
        editLoop sb newCursorPosition
        

// Função que salva o conteúdo em um arquivo
let saveFile filePath content = File.WriteAllText(filePath, content)

// Função principal que gerencia os comandos do usuário
let rec run filePath content =
    
    printfn "Digite 'salvar' para salvar, 'editar' para edição direta ou 'sair' para sair."
    let input = Console.ReadLine().Split([| ' ' |], 3)
    
    match input with
    
    | [| "salvar"; filePath; content |] ->
        
        File.WriteAllText(filePath, content)
        run filePath content
        
    | [| "sair" |] ->
        
        File.WriteAllText(filePath, content)
        printfn "Saindo..."
        
    | [| "editar" |] ->
        
        let initialContent = StringBuilder(content)
        let newContent = editLoop initialContent content.Length
        printfn "%s" newContent
        run filePath newContent
        
    | _ ->
        
        printfn "Comando inválido."
        run filePath content
