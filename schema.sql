  -- N2: uma simulação contém quadros (iterações); cada quadro contém os
  -- corpos ainda existentes naquele instante. Grave também a iteração 0.
  CREATE DATABASE IF NOT EXISTS simulador_gravitacional
      CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

  USE simulador_gravitacional;

  CREATE TABLE IF NOT EXISTS simulacoes (
      id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
      criada_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
      intervalo_ms INT UNSIGNED NOT NULL,
      delta_tempo_s DOUBLE NOT NULL,
      ultima_iteracao INT UNSIGNED NOT NULL DEFAULT 0,
      PRIMARY KEY (id)
  ) ENGINE = InnoDB;

  CREATE TABLE IF NOT EXISTS estados_corpos (
      simulacao_id BIGINT UNSIGNED NOT NULL,
      iteracao INT UNSIGNED NOT NULL,
      indice_corpo INT UNSIGNED NOT NULL,
      nome VARCHAR(255) NOT NULL,
      massa_kg DOUBLE NOT NULL,
      densidade_kg_m3 DOUBLE NOT NULL,
      pos_x DOUBLE NOT NULL,
      pos_y DOUBLE NOT NULL,
      vel_x_m_s DOUBLE NOT NULL,
      vel_y_m_s DOUBLE NOT NULL,
      cor_argb INT NOT NULL,
      PRIMARY KEY (simulacao_id, iteracao, indice_corpo),
      CONSTRAINT fk_estados_simulacao
          FOREIGN KEY (simulacao_id) REFERENCES simulacoes(id)
          ON DELETE CASCADE
  ) ENGINE = InnoDB;

  -- O índice composto permite carregar os quadros de uma simulação em ordem.
  -- SELECT * FROM estados_corpos WHERE simulacao_id = ? ORDER BY iteracao, indice_corpo;
