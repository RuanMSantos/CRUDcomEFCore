using CRUDcomEFCore.db;

bool sair = false;
while (!sair)
{
    string opcao = UI.SelecionaOpcaoEmMenu();

    switch (opcao)
    {
        case "L": ListarTodasAsTarefas(); break;
        case "P": ListarTarefasPendentes(); break;
        case "I": ListarTarefasPorId(); break;
        case "D": ListarTarefasPorDescricao(); break;
        case "N": IncluirNovaTarefa(); break;
        case "A": AlterarDescricaoDaTarefa(); break;
        case "C": ConcluirTarefa(); break;
        case "E": ExcluirTarefa(); break;

        case "S":
            sair = true;
            break;

        default:
            UI.ExibeErro("\nOpção não reconhecida.");
            break;
    }

    Console.Write("\nPressione uma tecla para continuar...");
    Console.ReadKey();
}

void ListarTodasAsTarefas()
{
    UI.ExibeDestaque("\n-- Listar todas as Tarefas ---");
    
    using (var _db = new TarefasContext()){
        
        var tarefas = _db.Tarefa.ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefa(s) encontrada(s).");

        foreach (var tarefa in tarefas){
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPendentes()
{
    UI.ExibeDestaque("\n-- Listar Tarefas Pendentes ---");
    
    using (var _db = new TarefasContext()){
        
        var tarefas = _db.Tarefa
            .Where(t => !t.Concluida)
            .OrderByDescending(t => t.Id)
            .ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefa(s) encontrada(s).");

        foreach (var tarefa in tarefas){
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPorDescricao()
{
    UI.ExibeDestaque("\n-- Listar Tarefas por Descrição ---");
    string descricao = UI.SelecionaDescricao();
    
    using (var _db = new TarefasContext()){
        
        var tarefas = _db.Tarefa
            .Where(t => t.Descricao.Contains(descricao))
            .OrderBy(t => t.Descricao)
            .ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefa(s) encontrada(s).");

        foreach (var tarefa in tarefas){
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPorId()
{
    UI.ExibeDestaque("\n-- Listar Tarefas por Id ---");
    int id = UI.SelecionaId();
    
    using (var _db = new TarefasContext()){
        
        var tarefa = _db.Tarefa.Find(id);

        if (tarefa is null){
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");    
    }
}

void IncluirNovaTarefa()
{
    UI.ExibeDestaque("\n-- Incluir Nova Tarefa ---");
    string descricao = UI.SelecionaDescricao();

    if (String.IsNullOrEmpty(descricao)){
        UI.ExibeErro("Não é possível incluir tarefa sem descrição.");
        return;
    }

    using (var _db = new TarefasContext()){
        
        var tarefa = new Tarefa{
            Descricao = descricao,
        };

        _db.Tarefa.Add(tarefa);
        _db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");    
    }
}

void AlterarDescricaoDaTarefa()
{
    UI.ExibeDestaque("\n-- Alterar Descrição da Tarefa ---");
    int id = UI.SelecionaId();
    string descricao = UI.SelecionaDescricao();

    if (String.IsNullOrEmpty(descricao)){
        UI.ExibeErro("Não é permitido deixar uma tarefa sem descrição.");
        return;
    }

    using (var _db = new TarefasContext()){
        
        var tarefa = _db.Tarefa.Find(id);

        if (tarefa is null){
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        tarefa.Descricao = descricao;
        _db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");    
    }
}

void ConcluirTarefa()
{
    UI.ExibeDestaque("\n-- Concluir Tarefa ---");
    int id = UI.SelecionaId();

    using (var _db = new TarefasContext()){
        
        var tarefa = _db.Tarefa.Find(id);

        if (tarefa is null){
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        if (tarefa.Concluida){
            UI.ExibeErro("Tarefa já concluída.");
            return;
        }

        tarefa.Concluida = true;
        _db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");    
    }
}

void ExcluirTarefa()
{
    UI.ExibeDestaque("\n-- Excluir Tarefa ---");
    int id = UI.SelecionaId();
    
    using (var _db = new TarefasContext()){
        
        var tarefa = _db.Tarefa.Find(id);

        if (tarefa is null){
            Console.WriteLine("Tarefa não encontrada.");
            return;
        }

        _db.Tarefa.Remove(tarefa);
        _db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");    
    }
}