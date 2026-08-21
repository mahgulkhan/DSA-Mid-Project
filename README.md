# The Campus Chronicle

A mini social media application designed for university students to share campus happenings, interact with each other, and view timelines with engagement features like likes and comments.

## Features

### User Features
- User registration and authentication with password hashing
- Create, view, and manage posts
- Like and comment on posts
- Search for users and posts
- Friend recommendations
- Navigation history with back tracking
- Undo functionality for actions
- View trending and recent posts

### Core Functionalities
- Queue-based chronological post feed
- Keyword search using linear search
- Sorting posts by time, likes, and alphabetically
- Stack-based navigation tracking
- Circular array for browsing history
- Friend management system

## Tech Stack

- **Language**: C# / .NET Framework
- **Database**: MySQL
- **UI Framework**: Windows Forms
- **Data Structures**: Linked List, Stack, Queue, Array

## Project Structure

```
The-Campus-Chronicle/
├── src/
│   ├── Forms/
│   │   ├── Login.cs
│   │   ├── Dashboard.cs
│   │   ├── PostsForm.cs
│   │   ├── SearchForm.cs
│   │   ├── ProfileForm.cs
│   │   └── FriendsForm.cs
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Post.cs
│   │   ├── Comment.cs
│   │   └── Friendship.cs
│   ├── DataStructures/
│   │   ├── LinkedList.cs
│   │   ├── Stack.cs
│   │   ├── Queue.cs
│   │   └── CircularArray.cs
│   ├── Database/
│   │   ├── DatabaseHelper.cs
│   │   └── Queries.sql
│   └── Utils/
│       ├── SessionManager.cs
│       └── NavigationStack.cs
├── database/
│   └── schema.sql
└── README.md
```

## Data Structures Used

| Structure | Purpose | Operations |
|-----------|---------|------------|
| **Linked List** | Store and manage user data | O(n) search, O(1) insertion |
| **Stack** | Track user navigation between forms | O(1) push/pop |
| **Queue** | Manage posts in chronological order | O(1) enqueue/dequeue |
| **Array (Circular)** | Track user actions for undo functionality | O(1) most operations |

## Algorithms Used

### Linear Search
- **Purpose**: Keyword-based search in posts
- **Complexity**: O(n)
- **Use Cases**: Post search, user search by username

### Bubble Sort
- **Purpose**: Sorting usernames alphabetically and posts by likes
- **Complexity**: O(n²)
- **Use Cases**: Trending posts, alphabetical user lists

### Selection Sort
- **Purpose**: Sorting posts by creation time (most recent)
- **Complexity**: O(n²)
- **Use Cases**: Recent posts feed

## Database Schema

### Users Table
```
user_id (PK)
username (UNIQUE)
password_hash
email
full_name
created_at
last_active
```

### Posts Table
```
post_id (PK)
user_id (FK)
content
like_count
comment_count
created_at
```

### Comments Table
```
comment_id (PK)
post_id (FK)
user_id (FK)
content
created_at
```

### Friendships Table
```
friendship_id (PK)
user_id (FK)
friend_id (FK)
status
created_at
```

## System Flow

### User Authentication
1. User enters credentials
2. Input validated against business rules
3. UserCRUD.Login() verifies credentials
4. Database queried for user verification
5. SessionManager.Login() sets user properties
6. User redirected to Dashboard

### Post Creation
1. User writes post content
2. Post object instantiated with metadata
3. Post added to PostQueue
4. Post saved to database
5. Feed updated to display posts

### Social Interactions
1. User clicks like/comment
2. Validation checks for duplicate actions
3. Counts updated in PostQueue
4. Database synced with like/comment record
5. History tracked for undo functionality

### Search & Discovery
1. User enters search keyword
2. PostsCrud.SearchPosts() traverses PostQueue
3. Matching posts collected
4. Posts sorted using Bubble Sort or Selection Sort
5. Results displayed in DataGridView

### Navigation Tracking
1. User navigates to new form
2. Current form data captured
3. Data pushed to NavigationStack
4. Stack maintains navigation trail
5. Pop retrieves previous form state

## Performance Analysis

### Time Complexity

| Operation | Data Structure | Time Complexity | Description |
|-----------|---------------|-----------------|-------------|
| User Login | Linked List | O(n) | Linear search by username |
| Add Post | Queue | O(1) | Enqueue operation |
| Search Posts | Queue + Linear Search | O(n) | Iterate through all posts |
| Sort Posts (Trending) | List + Bubble Sort | O(n²) | Compare all elements by likes |
| Sort Posts (Recent) | List + Selection Sort | O(n²) | Find most recent posts |
| Add Friend | Linked List | O(1) | Insert at end |
| Navigation Push/Pop | Stack | O(1) | Constant time operations |
| Undo Action | Array | O(1) | Access by index |

### Space Complexity

| Component | Space Complexity | Description |
|-----------|-----------------|-------------|
| User List | O(n) | n users in linked list |
| Post Queue | O(m) | m posts in queue |
| Comments | O(c) | c comments per post |
| Friends List | O(f) | f friendships |
| Navigation Stack | O(s) | s navigation states |
| Browsing History | O(1) | Fixed-size array |

## Key UI Screens

- **Launch Page**: Entry point with login/signup options
- **Login/Signup**: User authentication interface
- **Main Dashboard**: Central hub with feed and navigation
- **User Profile**: View and edit user information
- **Friends Page**: Manage friend connections
- **Posts Page**: Create and view posts
- **Search Page**: Find users and posts
- **Trending/Sorting**: Sort posts by engagement
- **Comments**: View and add comments to posts

## Installation

### Prerequisites
- Visual Studio 2019 or higher
- MySQL Server
- .NET Framework 4.5 or higher

### Setup

```bash
# Clone repository
git clone https://github.com/mahgulkhan/The-Campus-Chronicle.git

# Open solution in Visual Studio

# Update connection string in app.config
# Run database migrations
# Build and run the application
```
