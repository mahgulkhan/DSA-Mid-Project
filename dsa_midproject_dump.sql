CREATE DATABASE  IF NOT EXISTS `dsa_mid_project` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dsa_mid_project`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: dsa_mid_project
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `comment_id` int NOT NULL AUTO_INCREMENT,
  `post_id` int NOT NULL,
  `user_id` int NOT NULL,
  `content` text NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`comment_id`),
  KEY `post_id` (`post_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `comments_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `posts` (`post_id`) ON DELETE CASCADE,
  CONSTRAINT `comments_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
INSERT INTO `comments` VALUES (3,3,1,'ME toooo T_T','2025-11-23 07:04:51'),(4,2,1,'hi <3','2025-11-23 07:05:10'),(5,3,2,'same here :(','2025-11-23 07:06:43'),(6,4,2,'best of luck ^_^','2025-11-23 07:07:09'),(7,4,3,'oh damn','2025-11-23 07:08:23'),(8,1,3,'hello ','2025-11-23 07:08:39'),(9,2,3,'hello','2025-11-23 07:09:02'),(10,7,1,'?','2025-11-24 13:22:46');
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `friends`
--

DROP TABLE IF EXISTS `friends`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `friends` (
  `friendship_id` int NOT NULL AUTO_INCREMENT,
  `user_id1` int NOT NULL,
  `user_id2` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`friendship_id`),
  KEY `user_id1` (`user_id1`),
  KEY `user_id2` (`user_id2`),
  CONSTRAINT `friends_ibfk_1` FOREIGN KEY (`user_id1`) REFERENCES `users` (`user_id`) ON DELETE CASCADE,
  CONSTRAINT `friends_ibfk_2` FOREIGN KEY (`user_id2`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `friends`
--

LOCK TABLES `friends` WRITE;
/*!40000 ALTER TABLE `friends` DISABLE KEYS */;
INSERT INTO `friends` VALUES (1,1,2,'2025-11-22 10:07:08'),(2,1,3,'2025-11-23 11:22:18'),(3,1,5,'2025-11-25 01:49:08'),(4,1,6,'2025-11-25 01:49:20'),(5,1,7,'2025-11-25 01:49:32'),(6,1,8,'2025-11-25 01:49:45');
/*!40000 ALTER TABLE `friends` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `likes`
--

DROP TABLE IF EXISTS `likes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `likes` (
  `like_id` int NOT NULL AUTO_INCREMENT,
  `post_id` int NOT NULL,
  `user_id` int NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`like_id`),
  KEY `post_id` (`post_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `likes_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `posts` (`post_id`) ON DELETE CASCADE,
  CONSTRAINT `likes_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `likes`
--

LOCK TABLES `likes` WRITE;
/*!40000 ALTER TABLE `likes` DISABLE KEYS */;
INSERT INTO `likes` VALUES (5,3,1,'2025-11-23 07:04:41'),(6,2,1,'2025-11-23 07:05:23'),(8,3,2,'2025-11-23 07:06:30'),(9,3,3,'2025-11-23 07:07:49'),(10,1,3,'2025-11-23 07:07:53'),(11,2,3,'2025-11-23 07:07:58'),(12,4,3,'2025-11-23 07:08:05'),(13,3,1,'2025-11-24 09:10:12'),(16,4,1,'2025-11-24 09:31:23'),(17,1,1,'2025-11-24 09:33:21'),(18,4,1,'2025-11-24 09:34:30'),(21,7,4,'2025-11-24 11:32:11'),(23,2,4,'2025-11-24 11:32:21'),(24,1,4,'2025-11-24 11:32:28'),(25,8,1,'2025-11-24 11:33:53'),(27,14,6,'2025-11-24 11:54:30'),(29,1,6,'2025-11-24 11:54:40'),(30,12,6,'2025-11-24 11:54:55'),(31,13,6,'2025-11-24 11:55:01'),(32,9,6,'2025-11-24 11:55:08'),(34,20,6,'2025-11-24 20:57:59'),(35,21,7,'2025-11-24 21:33:17'),(36,22,7,'2025-11-24 21:33:23'),(37,23,1,'2025-11-25 01:51:19');
/*!40000 ALTER TABLE `likes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `posts`
--

DROP TABLE IF EXISTS `posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posts` (
  `post_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `content` text NOT NULL,
  `likes_count` int DEFAULT '0',
  `comment_count` int DEFAULT '0',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`post_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `posts_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posts`
--

LOCK TABLES `posts` WRITE;
/*!40000 ALTER TABLE `posts` DISABLE KEYS */;
INSERT INTO `posts` VALUES (1,1,'Hiii Guysss!',5,1,'2025-11-22 15:40:55'),(2,2,'Hey everyone',3,2,'2025-11-22 15:48:39'),(3,3,'So tired Lately',4,2,'2025-11-22 16:02:23'),(4,1,'OUR DSA PROJECTTT IS DUE ON MONDAYYY!!!!!!',3,2,'2025-11-22 18:09:30'),(7,2,'lol',1,1,'2025-11-24 11:23:34'),(8,4,'hello people',1,0,'2025-11-24 11:31:25'),(9,4,'wanna skip school',1,0,'2025-11-24 11:35:19'),(12,3,'nicee',1,0,'2025-11-24 11:37:14'),(13,5,'^0^',1,0,'2025-11-24 11:44:47'),(14,6,'uni is hectic',1,0,'2025-11-24 11:52:14'),(19,1,'thak gaya hu',0,0,'2025-11-25 01:48:03'),(20,5,'hello peeps',1,0,'2025-11-24 20:35:40'),(21,6,'bored rn',1,0,'2025-11-24 20:48:26'),(22,2,'I need to practice PF',1,0,'2025-11-24 20:59:23'),(23,7,'Chose BsIT as my dicipline',1,0,'2025-11-24 21:33:52'),(24,7,'could not get a scholarship:(',0,0,'2025-11-24 21:36:36'),(25,7,'need to improve my sleep schedule',0,0,'2025-11-24 21:49:29'),(26,3,'There is so much work on weekends',0,0,'2025-11-24 21:54:38'),(27,8,'Do not ignore your deadlines',0,0,'2025-11-24 22:01:51'),(28,8,'Speaking from personal experience',0,0,'2025-11-24 22:02:10'),(29,1,'any web develops here?',0,0,'2025-11-25 01:47:08');
/*!40000 ALTER TABLE `posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'mahgul','bcb15f821479b4d5772bd0ca866c00ad5f926e3580720659cc80d39c9d09802a','mahgul2310@gmail'),(2,'ayesha','4cc8f4d609b717356701c57a03e737e5ac8fe885da8c7163d3de47e01849c635','ayesha21@gmail'),(3,'fatima','af41e68e1309fa29a5044cbdc36b90a3821d8807e68c7675a6c495112bc8a55f','fatima@gmail'),(4,'ahmad','68487dc295052aa79c530e283ce698b8c6bb1b42ff0944252e1910dbecdc5425','ahmad@gmail'),(5,'hareem','91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203','hareem@gmail'),(6,'arij','3ea87a56da3844b420ec2925ae922bc731ec16a4fc44dcbeafdad49b0e61d39c','arij@gmail'),(7,'ariba','69f7f7a7f8bca9970fa6f9c0b8dad06901d3ef23fd599d3213aa5eee5621c3e3','goat@gmail'),(8,'areeba','ec4c88ca7f69534f10c0611c1ecd13e7c2cdf73e1b915e9fd0cf27ac10da43fa','areeba@gmail');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-11-25  7:58:06
