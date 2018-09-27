using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace JIMAssistant
{
    public partial class frmHome : Form
    {
        private SpeechSynthesizer falar = null; //Engine de fala
        private SpeechRecognitionEngine reconhecer = null; //Engine de reconhecimento

        public frmHome()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Fecha o sistema
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; //Mininiza o sistema
        }

        private bool mover;
        private int cX, cY;

        private void pnlBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cX = e.X;
                cY = e.Y;
                mover = true;
            }
        }

        private void pnlBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mover = false;
        }

        private void pnlBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mover)
            {
                this.Left += e.X - (cX - pnlBar.Left);
                this.Top += e.Y - (cY - pnlBar.Top);
            }
        }

        private void CarregarFala()
        {
            try
            {

                reconhecer = new SpeechRecognitionEngine( new System.Globalization.CultureInfo("pt-br")); //Instância
                reconhecer.SetInputToDefaultAudioDevice(); //Microfone
                falar = new SpeechSynthesizer(); //Instância
                falar.SetOutputToDefaultAudioDevice(); //Saída de Áudio
                falar.Rate = 1; //Velocidade de fala


                #region Comandos de Saudações

                //string[] saudacoes = { "Jim", "Bom dia", "Boa tarde", "Boa noite", "Não vou mais estudar hoje" };
                Choices c_saudacoes = new Choices();
                c_saudacoes.Add("Jim");
                c_saudacoes.Add("Bom dia");
                c_saudacoes.Add("Boa tarde");
                c_saudacoes.Add("Sair");

                GrammarBuilder gb_saudacoes = new GrammarBuilder();
                gb_saudacoes.Append(c_saudacoes);

                Grammar g_saudacoes = new Grammar(gb_saudacoes);
                g_saudacoes.Name = "saudações";

                #endregion

                #region Comandos de Pergunta Simples

                Choices c_perguntasSimples = new Choices();
                c_perguntasSimples.Add("Qual o seu objetivo");
                c_perguntasSimples.Add("Que horas são");
                c_perguntasSimples.Add("Que dia é hoje");
                c_perguntasSimples.Add("Quem te fez");
                c_perguntasSimples.Add("Quem é você");


                GrammarBuilder gb_perguntasSimples = new GrammarBuilder();
                gb_perguntasSimples.Append(c_perguntasSimples);

                Grammar g_perguntasSimples = new Grammar(gb_perguntasSimples);
                g_perguntasSimples.Name = "perguntas";

                #endregion

                #region Comandos de Matérias e Areas

                Choices c_materiasAreas = new Choices();
                c_materiasAreas.Add("Robótica");
                c_materiasAreas.Add("Matemática");
                c_materiasAreas.Add("Química");
                c_materiasAreas.Add("Inglês");

                GrammarBuilder gb_materiasAreas = new GrammarBuilder();
                gb_materiasAreas.Append(c_materiasAreas);

                Grammar g_materiasAreas = new Grammar(gb_materiasAreas);
                g_materiasAreas.Name = "matérias";

                #endregion

                #region Comando de Doenças

                Choices c_doencas = new Choices();
                c_doencas.Add("Gripe H1N1");
                c_doencas.Add("Dengue");
                c_doencas.Add("Tuberculose");

                GrammarBuilder gb_doencas = new GrammarBuilder();
                gb_doencas.Append(c_doencas);

                Grammar g_doencas = new Grammar(gb_doencas);
                g_doencas.Name = "doenças";

                #endregion

                #region Comandos de Pessoas Importantes

                Choices c_pessoasImportantes = new Choices();
                c_pessoasImportantes.Add("Quem foi Tiradentes");
                c_pessoasImportantes.Add("Quem foi Nelson Mandela");
                c_pessoasImportantes.Add("Quem foi Ayrton Senna");

                GrammarBuilder gb_pessoasImportantes = new GrammarBuilder();
                gb_pessoasImportantes.Append(c_pessoasImportantes);

                Grammar g_pessoasImportantes = new Grammar(gb_pessoasImportantes);
                g_pessoasImportantes.Name = "pessoas";

                #endregion

                #region Comandos de Outras Perguntas

                Choices c_outrasPerguntas = new Choices();
                c_outrasPerguntas.Add("Enem");
                c_outrasPerguntas.Add("Vestibulares");
                c_outrasPerguntas.Add("Redação");

                GrammarBuilder gb_outrasPerguntas = new GrammarBuilder();
                gb_outrasPerguntas.Append(c_outrasPerguntas);

                Grammar g_outrasPerguntas = new Grammar(gb_outrasPerguntas);
                g_outrasPerguntas.Name = "outras";

                #endregion

                #region Comando de Artigos

                Choices c_artigos = new Choices();
                c_artigos.Add("Artigo sobre política");
                c_artigos.Add("Artigo sobre o brasil");
                c_artigos.Add("Artigo sobre robôs");
                c_artigos.Add("Constituição");
                c_artigos.Add("Vídeos Enem 2018");

                GrammarBuilder gb_artigos = new GrammarBuilder();
                gb_artigos.Append(c_artigos);

                Grammar g_artigos = new Grammar(gb_artigos);
                g_artigos.Name = "artigos";

                #endregion

                reconhecer.RequestRecognizerUpdate(); //Atualizar o processo de reconhecimento

                //Carregar gramática de reconhecimento
                reconhecer.LoadGrammarAsync(g_saudacoes);
                reconhecer.LoadGrammarAsync(g_perguntasSimples);
                reconhecer.LoadGrammarAsync(g_materiasAreas);
                reconhecer.LoadGrammarAsync(g_doencas);
                reconhecer.LoadGrammarAsync(g_pessoasImportantes);
                reconhecer.LoadGrammarAsync(g_outrasPerguntas);
                reconhecer.LoadGrammarAsync(g_artigos);

                //Método para reconhecer a voz
                reconhecer.SpeechRecognized += reconhecimento;//new EventHandler<SpeechRecognizedEventArgs>(reconhecimento);

                //Método para reconhecer o tom da voz
                reconhecer.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(nivelAudio);
                reconhecer.RecognizeAsync(RecognizeMode.Multiple); //Iniciar reconhecimento
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu no CarregarFala(): " + ex.Message);
            }
        }

        private void reconhecimento(object s, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.5f)
            {
                string reconhecido = e.Result.Text;
                lblReconhecido.Text = " Você falou: ";
                lblReconhecido.Text += reconhecido;

                switch (e.Result.Grammar.Name)
                {
                    case "saudações":
                        processarSaudacoes(reconhecido);
                        break;

                    case "perguntas":
                        processarPerguntas(reconhecido);
                        break;

                    case "matérias":
                        processarMaterias(reconhecido);
                        break;

                    case "doenças":
                        processarDoencas(reconhecido);
                        break;

                    case "pessoas":
                        processarPessoas(reconhecido);
                        break;

                    case "outras":
                        processarOutras(reconhecido);
                        break;

                    case "artigos":
                        processarArtigos(reconhecido);
                        break;

                    default:
                        Fale("Infelizmente meu criador não adicionou esse comando!");
                        break;
                }
            }
            else
            {
                Fale("Não consegui entender o que você deseja! Fale um pouco mais alto!");
            }
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            CarregarFala();
            falar.SpeakAsync("Bem vindo! Ainda não estou 100% desenvolvida. Essa é uma demonstração.");
        }

        private void nivelAudio(object s, AudioLevelUpdatedEventArgs e)
        {
            this.nivelVoz.Maximum = 100;
            this.nivelVoz.Value = e.AudioLevel;
            nivelVoz.BackColor = Color.Black;
        }

        private void Fale(string texto)
        {
            if (falar.State == SynthesizerState.Speaking)
            {
                falar.SpeakAsyncCancelAll();
            }
            falar.SpeakAsync(texto);
        }

        private void processarSaudacoes(string saudacao)
        {
            if (saudacao.Equals("Jim"))
            {
                Fale("Olá, estou aqui para ajudá-lo!");
            }
            else if (saudacao.Equals("Bom dia"))
            {
                Fale("Bom dia aluno! Pósso te ajudar?");
            }
            else if (saudacao.Equals("Bom tarde"))
            {
                Fale("Bom tarde aluno! Pósso te ajudar?");
            }
            else if (saudacao.Equals("Bom noite"))
            {
                Fale("Bom noite aluno! Pósso te ajudar?");
            }
            else if (saudacao.Equals("Sair"))
            {
                Application.Exit();
            }
        }

        private void processarPerguntas(string pergunta)
        {
            if (pergunta.Equals("Quem é você"))
            {
                Fale("Sou Jim! Sua assistente educacional.");
            }
            else if (pergunta.Equals("Qual o seu objetivo"))
            {
                Fale("Fui criado para auxiliar você a estudar melhor.");
            }
            else if (pergunta.Equals("Que horas são"))
            {
                Fale("São " + DateTime.Now.ToShortTimeString());
            }
            else if (pergunta.Equals("Que dia é hoje"))
            {
                Fale("Hojé é " + DateTime.Now.ToLongDateString());
            }
            else if (pergunta.Equals("Quem te fez"))
            {
                Fale("Fui desenvolvida por um grupo de estudantes da escola Lía Sidou. Eles pensaram na ideia de criar um mecanismo capaz de ajudar alunos a estudarem melhor.");
            }
        }

        private void processarMaterias(string materias)
        {
            if (materias.Equals("Robótica"))
            {
                Fale("Robótica é a ciência e técnica da concepção, construção e utilização de robôs. Ela também engloba computadores e sistemas compostos por partes mecânicas motorizadas.");
            }
            else if (materias.Equals("Robótica"))
            {
                Fale("A matemática é a ciência do raciocínio lógico e abstrato, que estuda quantidades, medidas, espaços, estruturas, variações e estatísticas.");
            }
            else if (materias.Equals("Química"))
            {
                Fale("Química é a ciência que estuda a composição, estrutura, propriedades da matéria, as mudanças sofridas por ela durante as reações químicas e a sua relação com a energia");
            }
            else if (materias.Equals("Inglês"))
            {
                Fale("Inglês é uma língua germânica ocidental que surgiu nos reinos anglo-saxônicos da Inglaterra.");
            }
        }

        private void processarDoencas(string doenca)
        {
            if (doenca.Equals("Gripe H1N1"))
            {
                Fale("Gripe H1N1 é a conhecida gripe Suína. Infecção respiratória em humanos causada por uma cepa de influenza que surgiu pela primeira vez nos porcos.");
            }
            else if (doenca.Equals("Dengue"))
            {
                Fale("É uma doença viral transmitida por mosquitos que ocorre em áreas tropicais e subtropicais.");
            }
            else if (doenca.Equals("Tuberculose"))
            {
                Fale("É uma doença bacteriana infecciosa. Afeta principalmente os pulmões e pode ser grave.");
            }
        }

        private void processarPessoas(string pessoa)
        {
            if (pessoa.Equals("Quem foi tiradentes"))
            {
                Fale("Joaquim José da Silva Xavier, o Tiradentes, foi um dentista, tropeiro, minerador, comerciante, militar e ativista político que atuou no Brasil, mais especificamente nas capitanias de Minas Gerais e Rio de Janeiro.");
            }
            else if (pessoa.Equals("Quem foi Nelson Mandela"))
            {
                Fale("Nelson Mandela foi um advogado, líder rebelde e presidente da África do Sul de 1994 a 1999, considerado como o mais importante líder da África Negra, vencedor do Prêmio Nobel da Paz de 1993");
            }
            else if (pessoa.Equals("Quem foi Ayrton Senna"))
            {
                Fale("Ayrton Senna da Silva, ou simplesmente Senna, foi um piloto de Fórmula 1 das décadas de 80 e 90 e maior ídolo brasileiro do automobilismo.");
            }
        }

        private void processarOutras(string outra)
        {
            if (outra.Equals("Enem"))
            {
                Fale("O Exame Nacional do Ensino Médio, Enêm, é hoje o principal método de ingresso nas instituições públicas de nível superior.");
            }
            else if (outra.Equals("Vestibulares"))
            {
                Fale("As universidades e faculdades brasileiras utilizam diversos tipos de processo seletivo como forma de ingresso, desde a utilização da nota do Enem, sem a necessidade de fazer outras provas, até o exame vestibular tradicional e suas variações, como o Processo Seletivo Seriado e o Vestibular Agendado.");
            }
            else if (outra.Equals("Redação"))
            {
                Fale("Redação é o processo de redigir um texto. É uma atividade presente na cultura civilizada desde a invenção da escrita, e atualmente considerada um campo profissional e artístico na literatura, na produção de roteiros, na elaboração de relatórios e documentos.");
            }
        }

        private void processarArtigos(string outra)
        {
            if (outra.Equals("Artigo sobre política"))
            {
                System.Diagnostics.Process.Start("https://scholar.google.com.br/scholar?hl=pt-BR&q=política");
                Fale("Ok, vamos ver alguns artigos no Google Acadêmico, sobre política.");
            }
            else if (outra.Equals("Artigo sobre o brasil"))
            {
                System.Diagnostics.Process.Start("https://scholar.google.com.br/scholar?hl=pt-BR&q=brasil");
                Fale("Ok, vamos ver alguns artigos no Google Acadêmico, sobre brasil.");
            }
            else if (outra.Equals("Artigo sobre robôs"))
            {
                System.Diagnostics.Process.Start("https://scholar.google.com.br/scholar?hl=pt-BR&q=robôs");
                Fale("Ok, vamos ver alguns artigos no Google Acadêmico, sobre robôs.");
            }
            else if (outra.Equals("Constituição"))
            {
                System.Diagnostics.Process.Start("http://www.planalto.gov.br/ccivil_03/constituicao/constituicao.htm");
                Fale("Ok, vamos ver nossa constituição de 1988");
            }
            else if (outra.Equals("Vídeos Enem 2018"))
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/results?search_query=enem2018");
                Fale("Ok, vamos ver o que temos sobre Enem 2018 no youtube.");
            }
        }
    }
}