-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gazdă: 127.0.0.1
-- Timp de generare: iun. 10, 2023 la 02:13 PM
-- Versiune server: 10.4.28-MariaDB
-- Versiune PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Bază de date: `homematicdb`
--

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `actions`
--

CREATE TABLE `actions` (
  `action_id` int(11) NOT NULL,
  `device_id` varchar(16) NOT NULL,
  `action_type` varchar(50) DEFAULT NULL,
  `value_action` float DEFAULT NULL,
  `date_time` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `parameters`
--

CREATE TABLE `parameters` (
  `row_id` int(11) NOT NULL,
  `temperature` float NOT NULL,
  `light_intensity` int(11) NOT NULL,
  `opened_door` tinyint(1) NOT NULL,
  `current_preset` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `presets`
--

CREATE TABLE `presets` (
  `preset_id` int(11) NOT NULL,
  `preset_name` varchar(50) NOT NULL,
  `device_id` varchar(16) DEFAULT NULL,
  `option_code` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `temperature_esp`
--

CREATE TABLE `temperature_esp` (
  `id` int(11) NOT NULL,
  `timestamp_value` timestamp NOT NULL DEFAULT current_timestamp(),
  `temperature_ESP` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `users`
--

CREATE TABLE `users` (
  `device_id` varchar(16) NOT NULL,
  `passwrd` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `is_admin` tinyint(1) NOT NULL,
  `CNP` varchar(13) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexuri pentru tabele eliminate
--

--
-- Indexuri pentru tabele `actions`
--
ALTER TABLE `actions`
  ADD PRIMARY KEY (`action_id`),
  ADD KEY `device_id` (`device_id`);

--
-- Indexuri pentru tabele `parameters`
--
ALTER TABLE `parameters`
  ADD PRIMARY KEY (`row_id`);

--
-- Indexuri pentru tabele `presets`
--
ALTER TABLE `presets`
  ADD PRIMARY KEY (`preset_id`),
  ADD KEY `device_id` (`device_id`);

--
-- Indexuri pentru tabele `temperature_esp`
--
ALTER TABLE `temperature_esp`
  ADD PRIMARY KEY (`id`);

--
-- Indexuri pentru tabele `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`device_id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT pentru tabele eliminate
--

--
-- AUTO_INCREMENT pentru tabele `actions`
--
ALTER TABLE `actions`
  MODIFY `action_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pentru tabele `presets`
--
ALTER TABLE `presets`
  MODIFY `preset_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pentru tabele `temperature_esp`
--
ALTER TABLE `temperature_esp`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constrângeri pentru tabele eliminate
--

--
-- Constrângeri pentru tabele `actions`
--
ALTER TABLE `actions`
  ADD CONSTRAINT `actions_ibfk_1` FOREIGN KEY (`device_id`) REFERENCES `users` (`device_id`);

--
-- Constrângeri pentru tabele `presets`
--
ALTER TABLE `presets`
  ADD CONSTRAINT `presets_ibfk_1` FOREIGN KEY (`device_id`) REFERENCES `users` (`device_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
