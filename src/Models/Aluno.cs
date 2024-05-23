using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioTDD.Models
{
    public class Aluno
    {
        public Aluno(string nomeUsuario)
        {
            Nome = nomeUsuario;
            _cursos = new List<Curso>();
        }

        private string _nome;

        private List<Curso> _cursos;
        
        public string Nome
        {
            get => _nome;

            set
            {
                if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("O nome do usuário não pode ser vazio.");

                _nome = value;
            }
        }

        public void CadastrarCurso(Curso curso)
        {
            if(_cursos.Contains(curso))
                throw new ArgumentException("O curso já está cadastrado");
            
            _cursos.Add(curso);
        }

        public void RemoverCurso(Curso curso)
        {
            _cursos.Remove(curso);
        }

        public void AssistirAula(Curso curso, Aula aula, TimeSpan novoTempoVisto)
        {
            /*bool aulaNaoFoiEncontrada = curso.BuscarAula(aula.Titulo).Titulo == "Aula não encontrada";
            bool cursoNaoEstaCadastrado = !_cursos.Contains(curso);

            if(aulaNaoFoiEncontrada)
                throw new ArgumentException("Aula não encontrada nesse curso");
            
            if(cursoNaoEstaCadastrado)
                throw new ArgumentException("Curso não cadastrado no perfil");
            */

            try
            {
                Aula aulaNaoFoiEncontrada = curso.BuscarAula(aula.Titulo);
            }
            catch (Exception)
            {
                throw;
            }

            bool cursoNaoEstaCadastrado = !_cursos.Contains(curso);
            if(cursoNaoEstaCadastrado)
                throw new KeyNotFoundException("Curso não cadastrado perfil");

            aula.TempoVisto += novoTempoVisto;
            curso.AtualizarProgresso();
        }

        public Curso BuscarCurso(string cursoTitulo)
        {
            Curso cursoBusca = _cursos.Find(curso => curso.Titulo == cursoTitulo);
            
            if(cursoBusca != null)
                return cursoBusca;
            
            throw new KeyNotFoundException("Não há um curso cadastrado com esse nome");
        }

        public void ResponderQuestionario(Curso curso, Questionario questionario, string[] respostas)
        {
            BuscarCurso(curso.Titulo).BuscarQuestionario(questionario).Responder(respostas);
            curso.AtualizarProgresso();
        }
        
    }
}