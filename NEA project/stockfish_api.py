from stockfish import Stockfish
stockfish = Stockfish(r'C:\Users\namjo\OneDrive\Documents\Namjote.S.Dulay\stockfish_15.1_win_x64_avx2\stockfish-windows-2022-x86-64-avx2.exe')

f = open("C:\\Users\\namjo\\OneDrive\\Documents\\Namjote.S.Dulay\\A-levels\\Computer_Science\\Year 13\\NEA Namjote Dulay\\NEA project\\NEA project\\stockfish_input.txt", encoding="utf-8-sig")
fen = f.read()
f.close()
fen = fen.rstrip('\n')
stockfish.set_fen_position(fen)
best_move = stockfish.get_best_move()
best_move_arr = [best_move]
stockfish.make_moves_from_current_position(best_move_arr)
new_fen = stockfish.get_fen_position()
f = open("C:\\Users\\namjo\\OneDrive\\Documents\\Namjote.S.Dulay\\A-levels\\Computer_Science\\Year 13\\NEA Namjote Dulay\\NEA project\\NEA project\\stockfish_output.txt", "w")
f.write(new_fen)
f.close()