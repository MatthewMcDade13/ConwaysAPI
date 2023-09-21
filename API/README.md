# Conways-API
Conway's Game of Life implemented as an API


CREATE TABLE IF NOT EXISTS board (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    width INTEGER NOT NULL,
    height INTEGER NOT NULL,
)

CREATE TABLE IF NOT EXISTS board_cell (
    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    pos_x INTEGER NOT NULL,
    pos_y INTEGER NOT NULL,
    age INTEGER NOT NULL,
    is_alive BOOLEAN NOT NULL,
    board_id INTEGER NOT NULL,
    FOREIGN KEY (board_id)
        REFERENCES board (board_id)
        ON DELETE CASCADE
)


            var cell = board.GetCell(x, y);
            var cell = board.GetCell(x, y);

INSERT INTO board (width, height)
VALUES(64, 64)

INSERT INTO board_cell (pos_x, pos_y, n_generations, board_id)
VALUES(2, 5, 1, 1)


. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . X . . . . . . . . . 
. . . . . . . . . X X X . . . . . . . . 
. . . . . . . . . . X . . . . . . . . . 
. . . X . . . . . . . . . . . . . . . . 
. . X X X . . . . . . . . . . . . . . . 
. . . X . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . 