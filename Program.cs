using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Placa
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Functions f = new Functions();
			string sql = "server=localhost;database=estacionamento_veiculos;uid=root;pwd=;";

			using (MySqlConnection conect = new MySqlConnection(sql))
			{
				conect.Open();
                string vagasCount = "SELECT COUNT(*) FROM veiculos_ WHERE numero_placa IS NULL;";
                string ScriptBuscador = "SELECT numero_placa FROM veiculos_ WHERE numero_placa = @placa;";
                string ScriptBuscaId = "SELECT id FROM veiculos_ WHERE numero_placa = @placa;";
                string removePlaca = "UPDATE veiculos_ SET numero_placa = NULL WHERE id = @id;";
                string addPlaca = "UPDATE veiculos_ SET numero_placa = @placa WHERE numero_placa IS NULL LIMIT 1;";
                string table = "SELECT numero_placa FROM veiculos_ WHERE id = @id;";
                MySqlCommand count = new MySqlCommand(vagasCount, conect);
				MySqlCommand busca = new MySqlCommand(ScriptBuscador, conect);
				MySqlCommand id = new MySqlCommand(ScriptBuscaId, conect);
				MySqlCommand BuscaPlaca = new MySqlCommand(ScriptBuscador, conect);
				MySqlCommand tableVisu = new MySqlCommand(table, conect);
				try {
					int vagasTotal = 20;
					int vagasLivres = 0, op = 2;
					vagasLivres = Convert.ToInt32(count.ExecuteScalar());
					int value;
					string placaBuscar;
					string placa;
					while (op != 0)
					{
						if (vagasLivres > 0)
						{
							Console.Clear();
							Console.WriteLine($"-- Existem {vagasLivres} vagas livres --\n");
							Console.WriteLine("O que você gostaria de buscar em nosso estacionamento: \n");
							Console.WriteLine("[1] -> Saber onde meu carro esta estacionado");
							Console.WriteLine("[2] -> Reservar uma vaga para meu carro");
							Console.WriteLine("[3] -> Retirar meu carro");
							Console.WriteLine("[4] -> Vizualizar estacionamento");
							Console.WriteLine("[0] -> Para sair\n");
							Console.WriteLine("-------------------------------------------------------");
							value = int.Parse(Console.ReadLine());
							switch (value)
							{
								case 1:
									Console.Clear();
									Console.WriteLine("Para saber onde seu veículo está estacionado digite a placa do veículo: ");
									placaBuscar = Console.ReadLine();
									busca.Parameters.Clear();
									busca.Parameters.AddWithValue("@placa", placaBuscar.ToUpper());
									string buscador = busca.ExecuteScalar()?.ToString();
									if (buscador == null)
									{
										Console.Clear();
										Console.WriteLine("Veiculo não encontrado!!");
										Console.WriteLine($"-- Ainda possuem {vagasLivres} vagas Livres --\n");
										Console.WriteLine("Digite: \n");
										Console.WriteLine("[1] -> Para para reservar uma vaga para seu veículo");
										Console.WriteLine("[2] -> Voltar ao Menu principal ");
										Console.WriteLine("-------------------------------------------------------");
										int r = int.Parse(Console.ReadLine());
										while (r != 1 && r != 2)
										{
											Console.Clear();
											Console.WriteLine("Opção selecionada não encontrada, deseja realizar qual ação: \n");
											Console.WriteLine("[1] -> Para para reservar uma vaga para seu veículo");
											Console.WriteLine("[2] -> Voltar ao Menu principal ");
											Console.WriteLine("-------------------------------------------------------");
											r = int.Parse(Console.ReadLine());
										}
										if (r == 1)
										{
											Console.Clear();
											Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
											placa = Console.ReadLine();
											await f.ReservarVaga(addPlaca, placa);
											vagasLivres--;
											Console.Clear();
											Console.WriteLine("Voltando para o Menu principal...");
											await Task.Delay(2000);
										}
										else if (r == 2)
										{
											Console.Clear();
											Console.WriteLine("Voltando para o menu principal...");
											await Task.Delay(2000);
											break;

										}
									}
									else if (buscador != null)
									{
										Console.Clear();
										id.Parameters.Clear();
										id.Parameters.AddWithValue("@placa", placaBuscar.ToUpper());
										int mostrador2 = Convert.ToInt32(id.ExecuteScalar());
										Console.WriteLine($"Esse veiculo está ocupando a vaga de n° {mostrador2}!\n");
										Console.WriteLine("Digite: ");
										Console.WriteLine("[1] -> Para retirar seu veículo dessa vaga");
										Console.WriteLine("[2] -> Voltar ao Menu principal ");
										Console.WriteLine("-------------------------------------------------------");
										int r1 = int.Parse(Console.ReadLine());
										if (r1 == 1)
										{
											Console.Clear();
											Console.WriteLine($"Deseja realmente tirar o veículo de placa {placaBuscar}, localizado na vaga {mostrador2} ?");
											Console.WriteLine("[S] -> Sim");
											Console.WriteLine("[N] -> Não");
											Console.WriteLine("-------------------------------------------------------");
											string r2 = (Console.ReadLine());
											r2 = r2.ToUpper();
											if (r2 == "S" || r2 == "SIM")
											{
												await f.RetirarCarro(removePlaca, placaBuscar, mostrador2);
												vagasLivres = vagasLivres + 1;
												await Task.Delay(2000);
												Console.WriteLine("Voltando para o menu principal...");
												break;
											}
										}
										else if (r1 == 2)
										{
											Console.Clear();
											Console.WriteLine("Voltando para o menu principal...");
											await Task.Delay(2000);
											break;
										}
										else
										{
											Console.Clear();
											Console.WriteLine("Opção selecionada não encontrada, deseja realizar qual ação: ");
											Console.WriteLine("[1] -> Para para reservar uma vaga para seu veículo");
											Console.WriteLine("[2] -> Voltar ao Menu principal ");
											Console.WriteLine("-------------------------------------------------------");
											int r3 = int.Parse(Console.ReadLine());
											if (r3 == 1)
											{
												Console.Clear();
												Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
												placa = Console.ReadLine();
												await f.ReservarVaga(addPlaca, placa);
												Console.Clear();
												Console.WriteLine("Voltando para o Menu principal...");
												await Task.Delay(2000);
											}
											else
											{
												Console.Clear();
												break;
											}
										}
									}
									break;
								case 2:
									while (true)
									{
										Console.Clear();
										Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
										string placa1 = Console.ReadLine();
										busca.Parameters.Clear();
										busca.Parameters.AddWithValue("@placa", placa1.ToUpper());
										string buscador1 = busca.ExecuteScalar()?.ToString();
										if (buscador1 == null)
										{
											Console.Clear();
											await f.ReservarVaga(addPlaca, placa1);
											vagasLivres = vagasLivres - 1;
											Console.WriteLine("[1] - Voltar para o menu principal");
											Console.WriteLine("[2] - Adicionar um novo veiculo");
											Console.WriteLine("-------------------------------------------------------");
											Console.WriteLine("Digite: ");
											int menu = int.Parse(Console.ReadLine());
											if (menu == 1)
											{
												break;
											}
											else if (menu == 2)
											{
												await Task.Delay(5000);
											}
											else
											{
												break;
											}

										}
										else if (buscador1 != null)
										{
											Console.Clear();
											id.Parameters.Clear();
											id.Parameters.AddWithValue("@placa", buscador1);
											int mostrador3 = Convert.ToInt32(id.ExecuteScalar());
											Console.WriteLine($"Esse veiculo está ocupando a vaga de n° {mostrador3 + 1}!\n");
											Console.WriteLine("Digite: ");
											Console.WriteLine("[1] -> Para retirar seu veículo dessa vaga");
											Console.WriteLine("[2] -> Voltar ao Menu principal ");
											Console.WriteLine("-------------------------------------------------------");
											int r1 = int.Parse(Console.ReadLine());
											if (r1 == 1)
											{
												Console.Clear();
												Console.WriteLine($"Deseja realmente tirar o veículo de placa {placa1}, localizado na vaga {mostrador3 + 1} ?");
												Console.WriteLine("[S] -> Sim");
												Console.WriteLine("[N] -> Não");
												Console.WriteLine("-------------------------------------------------------");
												string r2 = (Console.ReadLine());
												r2 = r2.ToUpper();
												if (r2 == "S" || r2 == "SIM")
												{
													await f.RetirarCarro(removePlaca, placa1, mostrador3);
													vagasLivres = vagasLivres + 1;
													await Task.Delay(2000);
													Console.WriteLine("Voltando para o menu principal...");
													break;
												}
											}
											else if (r1 == 2)
											{
												Console.Clear();
												Console.WriteLine("Voltando para o menu principal...");
												await Task.Delay(2000);
												break;
											}
											else
											{
												Console.Clear();
												Console.WriteLine("Opção selecionada não encontrada, deseja realizar qual ação: ");
												Console.WriteLine("[1] -> Para para reservar uma vaga para seu veículo");
												Console.WriteLine("[2] -> Voltar ao Menu principal ");
												Console.WriteLine("-------------------------------------------------------");
												int r3 = int.Parse(Console.ReadLine());
												if (r3 == 1)
												{
													Console.Clear();
													Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
													placa = Console.ReadLine();
													await f.ReservarVaga(addPlaca, placa1);
													vagasLivres = vagasLivres - 1;
													Console.Clear();
													Console.WriteLine("Voltando para o Menu principal...");
													await Task.Delay(2000);
													break;
												}
												else
												{
													Console.Clear();
													break;
												}
											}
										}
										else
										{
											Console.WriteLine("Voltando ao menu principal...");
											await Task.Delay(2000);
											break;
										}
									}
									break;
								case 3:
									while (true)
									{
										Console.Clear();
										Console.WriteLine("Para retirar o seu carro da vaga, digite a placa do veículo: ");
										string placa3 = Console.ReadLine();
										busca.Parameters.Clear();
										busca.Parameters.AddWithValue("@placa", placa3);
										string buscador3 = busca.ExecuteScalar()?.ToString();
                                        busca.Parameters.Clear();
                                        busca.Parameters.AddWithValue("@placa", placa3.ToUpper());
                                        if (buscador3 != null)
                                        {
                                            id.Parameters.Clear();
                                            id.Parameters.AddWithValue("@placa", placa3.ToUpper());
                                            int mostrador3 = Convert.ToInt32(id.ExecuteScalar());

                                            Console.Clear();
                                            Console.WriteLine($"Deseja realmente remover o veículo {placa3} dessa vaga? ");
                                            Console.WriteLine("[S] -> Sim");
											Console.WriteLine("[N] -> Não");
											Console.WriteLine("-------------------------------------------------------");
											string r2 = (Console.ReadLine());
											r2 = r2.ToUpper();
											if (r2 == "S" || r2 == "SIM")
											{
												await f.RetirarCarro(removePlaca, placa3, mostrador3);
												vagasLivres = vagasLivres + 1;
												await Task.Delay(2000);
												Console.WriteLine("Voltando para o menu principal...");
												break;
											}
											else { 												
												Console.Clear();
												Console.WriteLine("Voltando ao menu principal...");
												await Task.Delay(2000);
												break;
                                            }
                                        }
										else
										{
											Console.Clear();
											Console.WriteLine("Veiculo não encontrado!!");
											Console.WriteLine($"-- Ainda possuem {vagasLivres} vagas Livres --\n");
											Console.WriteLine("[1] -> Para para reservar uma vaga para seu veículo");
											Console.WriteLine("[2] -> Voltar ao Menu principal ");
											Console.WriteLine("-------------------------------------------------------");
											Console.WriteLine("Digite: \n");
											int r = int.Parse(Console.ReadLine());
											if (r == 1)
											{
												Console.Clear();
												Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
												placa3 = Console.ReadLine();
												await f.ReservarVaga(addPlaca, placa3);
												vagasLivres = vagasLivres - 1;
												Console.Clear();
												Console.WriteLine("Voltando para o Menu principal...");
												await Task.Delay(2000);
												break;
											}
											else if (r == 2)
											{
												Console.Clear();
												Console.WriteLine("Voltando para o menu principal...");
												await Task.Delay(2000);
												break;
											}
											else
											{
												Console.Clear();
												Console.WriteLine("Opção selecionada não encontrada, deseja realizar qual ação: \n");
												Console.WriteLine("[1] -> Para reservar uma vaga para seu veículo");
												Console.WriteLine("[2] -> Voltar ao Menu principal ");
												Console.WriteLine("-------------------------------------------------------");
												r = int.Parse(Console.ReadLine());
											}
										}
									}
									break;
								case 4:
									Console.Clear();
									Console.WriteLine("-- Estacionamento --");
									for (int i = 0; i < vagasTotal; i++)
									{
										tableVisu.Parameters.Clear();
										tableVisu.Parameters.AddWithValue("@id", i + 1);
										string placaVisu = tableVisu.ExecuteScalar()?.ToString();
										if (string.IsNullOrEmpty(placaVisu))
										{
											Console.WriteLine($"[{i + 1}] - Vaga Livre");
										}
										else
										{
											Console.WriteLine($"[{i + 1}] - {placaVisu}");
										}
									}
									Console.WriteLine("-------------------------------------------------------");
									Console.WriteLine("[1] -> Reservar uma nova vaga");
									Console.WriteLine("[2] -> Retirar um veiculo");
									Console.WriteLine("[3] -> Voltar ao menu principal");
									Console.WriteLine("Digite: ");
									int r4 = int.Parse(Console.ReadLine());

									switch (r4)
									{
										case 1:
											Console.Clear();
											Console.WriteLine("Para reservar uma vaga, digite a placa do veiculo: ");
											placa = Console.ReadLine();
											await f.ReservarVaga(addPlaca, placa);
											vagasLivres = vagasLivres - 1;
											Console.Clear();
											Console.WriteLine("Voltando para o Menu principal...");
											await Task.Delay(2000);
											break;
										case 2:
											Console.Clear();
											Console.WriteLine("Para retirar o seu carro da vaga, digite a placa do veículo: ");
											string placa2 = Console.ReadLine();
											Console.Clear();
											Console.WriteLine($"Deseja realmente remover o veículo {placa2} dessa vaga? ");
											Console.WriteLine("[S] -> Sim");
											Console.WriteLine("[N] -> Não");
											Console.WriteLine("-------------------------------------------------------");
											string r2 = (Console.ReadLine());
											r2 = r2.ToUpper();
											if (r2 == "SIM")
											{
												Console.Clear();
												id.Parameters.Clear();
												id.Parameters.AddWithValue("@placa", placa2);
												int mostrador3 = Convert.ToInt32(id.ExecuteScalar());
												await f.RetirarCarro(removePlaca, placa2, mostrador3);
												vagasLivres = vagasLivres + 1;
												Console.WriteLine("Voltando para o menu principal...");
												await Task.Delay(2000);
												break;
											}
											else
											{
												Console.WriteLine("Voltando ao menu principal...");
												await Task.Delay(2000);
												break;
											}
										case 3:
											Console.Clear();
											Console.WriteLine("Voltando para o menu principal...");
											await Task.Delay(2000);
											break;
										default:
											Console.Clear();
											Console.WriteLine("Opção selecionada não encontrada, deseja realizar qual ação: \n");
											Console.WriteLine("[1] -> Para reservar uma vaga para seu veículo");
											Console.WriteLine("[2] -> Voltar ao Menu principal ");
											Console.WriteLine("-------------------------------------------------------");
											int resposta = int.Parse(Console.ReadLine());
											if (resposta == 1)
											{
												r4 = 1;
											}
											else if (resposta	 == 2)
											{
												Console.Clear();
												Console.WriteLine("Voltando para o menu principal...");
												await Task.Delay(2000);
												break;
											}
											break;
									}
									break;
								case 0:
									Console.Clear();
									Console.WriteLine("Finalizando o sistema...");
									await Task.Delay(2000);
									Console.Clear();
									return;
							}
						}

						else
						{
							Console.Clear();
							Console.WriteLine("-- Todas as vagas estão ocupadas no momento --");
							Console.WriteLine("Digite: ");
							Console.WriteLine("[1] -> Para remover um veículo");
							Console.WriteLine("[2] -> Visualizar estacionamento");
                            Console.WriteLine("[3] - > Para finalizar o sistema");
							Console.WriteLine("-------------------------------------------------------");
							int r5 = int.Parse(Console.ReadLine());
							switch (r5)
							{
								case 1:
									Console.Clear();
									Console.WriteLine("Para retirar o seu carro da vaga, digite a placa do veículo: ");
									string placa4 = Console.ReadLine();
									busca.Parameters.Clear();
									busca.Parameters.AddWithValue("@placa", placa4.ToUpper());
									string buscador3 = busca.ExecuteScalar()?.ToString();
									id.Parameters.Clear();
									id.Parameters.AddWithValue("@placa", placa4.ToUpper());
									id.ExecuteNonQuery();
									int mostrador4 = Convert.ToInt32(id.ExecuteScalar());
									if (buscador3 != null)
									{
										Console.Clear();
										Console.WriteLine($"Deseja realmente remover o veículo {placa4} dessa vaga? ");
										Console.WriteLine("[S] -> Sim");
										Console.WriteLine("[N] -> Não");
										Console.WriteLine("-------------------------------------------------------");
										string r2 = (Console.ReadLine());
										r2 = r2.ToUpper();
										if (r2 == "S" || r2 == "SIM")
										{
											await f.RetirarCarro(removePlaca, placa4, mostrador4);
											vagasLivres = vagasLivres + 1;
											await Task.Delay(2000);
											Console.WriteLine("Abrindo menu principal...");
											break;
										}
									}
									else
									{
										Console.WriteLine("Veículo não encontrado, voltando ao menu principal...");
										r5 = 1;
									}
									break;
								case 2:
                                    Console.Clear();
                                    Console.WriteLine("-- Estacionamento --");
                                    for (int i = 0; i < vagasTotal; i++)
                                    {
                                        tableVisu.Parameters.Clear();
                                        tableVisu.Parameters.AddWithValue("@id", i + 1);
                                        string placaVisu = tableVisu.ExecuteScalar()?.ToString();
                                        if (string.IsNullOrEmpty(placaVisu))
                                        {
                                            Console.WriteLine($"[{i + 1}] - Vaga Livre");
                                        }
                                        else
                                        {
                                            tableVisu.Parameters.Clear();
                                            tableVisu.ExecuteNonQuery();
                                            Console.WriteLine($"[{i + 1}] - {placaVisu}");
                                        }
                                    }
                                    Console.WriteLine("-------------------------------------------------------");
                                    Console.WriteLine("[1] -> Retirar um veiculo");
                                    Console.WriteLine("[2] -> Voltar ao menu principal");
                                    Console.WriteLine("Digite: ");
                                    int r4 = int.Parse(Console.ReadLine());
									if (r4 == 1) { 
										Console.Clear();
										Console.WriteLine("Digite a placa do veiculo para retira-lo: ");
										string placa5 = Console.ReadLine();
										busca.Parameters.Clear();
										busca.Parameters.AddWithValue("@placa", placa5.ToUpper());
										string buscador4 = busca.ExecuteScalar()?.ToString();
										if (buscador4 == null) { 
											Console.WriteLine("Veículo não encontrado, voltando ao menu principal...");
											await Task.Delay(2000);
											break;
                                        }
										else
										{
											id.Parameters.Clear();
											id.Parameters.AddWithValue("@placa", placa5.ToUpper());
											int mostrador5 = Convert.ToInt32(id.ExecuteScalar());
											Console.Clear();
											Console.WriteLine($"Deseja realmente remover o veículo {placa5} dessa vaga? ");
											Console.WriteLine("[S] -> Sim");
											Console.WriteLine("[N] -> Não");
											Console.WriteLine("-------------------------------------------------------");
											string r2 = (Console.ReadLine());
											r2 = r2.ToUpper();
											if (r2 == "S" || r2 == "SIM")
											{
												await f.RetirarCarro(removePlaca, placa5, mostrador5);
												vagasLivres = vagasLivres + 1;
												await Task.Delay(2000);
												Console.WriteLine("Abrindo menu principal...");
												break;
                                            }
                                        }
                                    }
									break;
									case 3:
									Console.Clear();
									return;
							}
						}
					}

				}
                catch (Exception erro)
                {
                    Console.Clear();
                    Console.WriteLine("Erro na conexao com banco de dados!");
                    Console.WriteLine($"Erro: {erro}");
                    await Task.Delay(5000);
                }
            } 
				
			}
		}
	} 
