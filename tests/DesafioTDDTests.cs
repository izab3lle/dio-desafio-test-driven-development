using DesafioTDD.Models;

namespace tests;

public class AlunoTests
{

    [Fact]
    public void CadastrarAluno_ComNomeVazio_DeveLancarArgumentException()
    {
        //Arrange
        Curso curso1 = new Curso("Curso 1");

        //Act + Assert
        Assert.Throws<ArgumentException>(
            () => curso1.Alunos.Add(new Aluno(""))
        );
    }

    [Fact]
    public void CadastrarCurso_ComNomeVaizo_DeveLancarArgumentException()
    {      
        //Arrange
        Aluno aluno = new Aluno("Aluno 1");

        //Assert
        Assert.Throws<ArgumentException>(
            () => aluno.CadastrarCurso(new Curso(""))
        );
    }

    [Theory]
    [InlineData(0, 20, 40)]
    [InlineData(1, 0, 33)]
    [InlineData(0, 45, 12)]
    [InlineData(0, 10, 5)]
    public void AssistirAula_DeveMudarTempoAssistido(int horas, int minutos, int segundos)
    {
        //Arrange
        TimeSpan duracaoTotalDaAula = new TimeSpan(1, 30, 0);
        TimeSpan tempoVisto = new TimeSpan(horas, minutos, segundos);
        
        Curso curso = new Curso("Curso 1");
        Aula aula = new Aula(titulo: "Aula 1", tempoTotal: duracaoTotalDaAula);
        Aluno aluno = new Aluno("Aluno 1");
        
        var resultadoEsperado = tempoVisto;

        //Act
        aluno.CadastrarCurso(curso);
        curso.CadastrarAula(aula);
        aluno.AssistirAula(curso, aula, tempoVisto);
        
        var resultado = curso.BuscarAula(aula.Titulo).TempoVisto;

        //Assert
        Assert.Equal(resultadoEsperado, resultado);
    }

    [Fact]
    public void AssistirAula_InserindoTempoNegativo_DeveLancarException()
    {
        //Arrange
        TimeSpan tempoNegativo = new TimeSpan(0, -20, 0);
        TimeSpan duracaoAula = new TimeSpan(0, 50, 0);
        Curso curso = new Curso("Curso 1");
        Aula aula = new Aula(titulo: "Aula 1", tempoTotal: duracaoAula);
        Aluno aluno = new Aluno("Aluno 1");

        //Act
        curso.CadastrarAula(aula);
        aluno.CadastrarCurso(curso);

        //Assert
        Assert.Throws<ArgumentException>(
            () => aluno.AssistirAula(curso, aula, novoTempoVisto: tempoNegativo)
        );
    }

    [Fact]
    public void AssistirAula_DeUmCursoQueNaoPossui_DeveLancarException()
    {
        //Arrange
        TimeSpan duracaoAula = new TimeSpan(0, 50, 0);
        TimeSpan tempoVisto = new TimeSpan(0, 30, 0);
        Curso curso = new Curso("Curso 1");
        Aula aula = new Aula(titulo: "Aula 1", tempoTotal: duracaoAula);
        Aluno aluno = new Aluno("Aluno 1");

        //Act
        curso.CadastrarAula(aula);

        //Assert
        Assert.Throws<KeyNotFoundException>(
            () => aluno.AssistirAula(curso, aula, novoTempoVisto: tempoVisto)
        );
    }

    [Fact]
    public void ResponderQuestioario_AcertarTodasAsPerguntas_DeveRetornarResultado100()
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        string[] respostas = new string[3];

        Questionario questionario = new Questionario();
        for(int i = 0; i < 3; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
            respostas[i] = $"Resposta {i + 1}";
        }

        //Act
        curso.Questionarios.Add(questionario);
        aluno.CadastrarCurso(curso);
        aluno.ResponderQuestionario(curso, questionario, respostas);

        //Assert
        Assert.Equal("100%", questionario.ResultadoPorcentagem);

    }

    [Fact]
    public void ResponderQuestionario_AcertarMetadeDasPerguntas_DeveRetornarResultado50()
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        string[] respostas = new string[4];

        Questionario questionario = new Questionario();
        for(int i = 0; i < 4; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
            respostas[i] = $"Resposta {i + 1}";
        }

        respostas[2] = "";
        respostas[3] = "";

        //Act
        curso.Questionarios.Add(questionario);
        aluno.CadastrarCurso(curso);
        aluno.ResponderQuestionario(curso, questionario, respostas);

        //Assert
        Assert.Equal("50%", questionario.ResultadoPorcentagem);
    }

    [Fact]
    public void ResponderQuestionario_ErrarTodasAsPerguntas_DeveRetornarResultado0()
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        string[] respostas = new string[4];

        Questionario questionario = new Questionario();
        for(int i = 0; i < 3; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
            respostas[i] = "x";
        }

        //Act
        curso.Questionarios.Add(questionario);
        aluno.CadastrarCurso(curso);
        aluno.ResponderQuestionario(curso, questionario, respostas);

        //Assert
        Assert.Equal("0%", questionario.ResultadoPorcentagem);
    }

    [Fact]
    public void ResponderQuestionario_RespostasVazias_DeveLancarException()
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        string[] respostas = new string[1];

        Questionario questionario = new Questionario();
        for(int i = 0; i < 3; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
        }

        //Act
        curso.Questionarios.Add(questionario);
        aluno.CadastrarCurso(curso);

        //Assert
        Assert.Throws<ArgumentException>(
            () => aluno.ResponderQuestionario(curso, questionario, respostas)
        );
    }

    [Fact]
    public void ConsultarHistorico_DeveRetornarHistoricoDoQuestionario()
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        Questionario questionario = new Questionario();

        for(int i = 0; i < 4; i++)
        {
            questionario.AdicionarQuestao($"Pergunta {i + 1}", "V");
        }

        string[] respostas1 = {"V", "V", "V", "V"};
        string[] respostas2 = {"V", "F", "V", "F"};
        string[] respostas3 = {"F", "F", "F", "F"};

        List<string> resultadoEsperado = new List<string> {"100%", "50%", "0%"};

        //Act
        aluno.CadastrarCurso(curso);
        curso.Questionarios.Add(questionario);

        aluno.ResponderQuestionario(curso, questionario, respostas1);
        aluno.ResponderQuestionario(curso, questionario, respostas2);
        aluno.ResponderQuestionario(curso, questionario, respostas3);

        //Assert
        Assert.Equal(resultadoEsperado, questionario.HistoricoResultados);
    }

    [Fact]
    public void ConcluirCurso_ComMediaAcimaDe65_AtributoConcluidoDeveSerTrue()
    {
        //Arange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        TimeSpan tempoDeAula = new TimeSpan(0, 30, 0);
        Aula aula = new Aula("Aula 1", tempoTotal: tempoDeAula);
        Questionario questionario = new Questionario();
        string[] respostas = new string[4];
        var resultadoEsperado = true;

        for(int i = 0; i < 4; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
            respostas[i] = $"Resposta {i + 1}";
        }

        //Act
        curso.Questionarios.Add(questionario);
        curso.CadastrarAula(aula);
        aluno.CadastrarCurso(curso);
        aluno.ResponderQuestionario(curso, questionario, respostas);
        aluno.AssistirAula(curso, aula, tempoDeAula);
        var resultado = curso.Concluido;

        //Assert
        Assert.Equal(resultadoEsperado, resultado);
    }

    [Fact]
    public void ConcluirCurso_ComMediaAbaixoDe65_AtributoConcluidoDeveSerFalse()
    {
        //Arange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        TimeSpan tempoDeAula = new TimeSpan(0, 30, 0);
        Aula aula = new Aula("Aula 1", tempoTotal: tempoDeAula);
        Questionario questionario = new Questionario();
        string[] respostas = new string[4];
        var resultadoEsperado = false;

        for(int i = 0; i < 4; i++)
        {
            questionario.AdicionarQuestao($"Questão {i + 1}", $"Resposta {i + 1}");
            respostas[i] = " ";
        }

        //Act
        curso.Questionarios.Add(questionario);
        curso.CadastrarAula(aula);
        aluno.CadastrarCurso(curso);
        aluno.ResponderQuestionario(curso, questionario, respostas);
        aluno.AssistirAula(curso, aula, tempoDeAula);
        var resultado = curso.Concluido;

        //Assert
        Assert.Equal(resultadoEsperado, resultado);
    }

    [Theory]
    [InlineData(0, 30, 44)]
    [InlineData(1, 2, 37)]
    [InlineData(0, 22, 44)]
    [InlineData(0, 45, 58)]
    [InlineData(0, 18, 20)]
    public void AssistirAula_ConcluirAulaInteira_AtributoConcluidoDeveSerTrue(int horas, int minutos, int segundos)
    {
        //Arrange
        Curso curso = new Curso("Curso 1");
        Aluno aluno = new Aluno("Aluno 1");
        TimeSpan tempoAula = new TimeSpan(horas, minutos, segundos);
        Aula aula = new Aula("Aula 1", tempoTotal: tempoAula);
        var resultadoEsperado = true;

        //Act
        curso.CadastrarAula(aula);
        aluno.CadastrarCurso(curso);
        aluno.AssistirAula(curso, aula, tempoAula);
        var resultado = aula.Concluida;

        //Assert
        Assert.Equal(resultadoEsperado, resultado);
    }    

}