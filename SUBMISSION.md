# Submission Instructions for Recipe Discovery Challenge

Since this repository (`recipe-discovery-challenge`) is private, you can’t fork it directly on GitHub. Instead, you’ll clone it locally, create your own private repository, work on your solution, and share it with me by inviting me as a collaborator. Follow these steps to complete and submit your work.

---

## Step 1: Clone the Repository Locally
You’ll need to download the challenge files to your computer.

1. **Get the Clone URL**:
   - Visit `https://github.com/developer-tii/recipe-discovery-challenge`.
   - Click the green **Code** button and copy the HTTPS URL (e.g., `https://github.com/developer-tii/recipe-discovery-challenge.git`).
2. **Open a Terminal**:
   - Use Git Bash, PowerShell, or your IDE’s terminal.
3. **Clone the Repo**:
   - Navigate to your workspace:
     ```bash
     cd /path/to/your/workspace
     Run:
     git clone https://github.com/developer-tii/recipe-discovery-challenge.git
     Enter your GitHub credentials if propmpted (username and password or a personal access token).

4. **Navigate into the Folder:**
    ```bash
    cd recipe-discovery-challenge

## Step 2: Create Your Own Private Repository
Create a new private repo under your GitHub account to host your solution.

1. **Log in to GitHub:**
    - Go to github.com and sign in.
2. **Create a New Repository:**
    - Click the + icon in the top-right corner and select New respository.
    - Repository Name: E.g., recipe-discovery-solution (or any name you choose).
    - Description (optional): "My solution to the Recipe Discovery challenge."
    - Public/Private: select Private.
    - Initialize: Leave all checkboxes unchecked (no README, .gitignore, or license needed yet).
    - Click Create repository.
3. **Copy Your Repo URL:**
    - On your new repo’s page (e.g., https://github.com/yourusername/recipe-discovery-solution), click the green Code button.
    - Copy the HTTPS URL (e.g., https://github.com/yourusername/recipe-discovery-solution.git).

## Step 3: Push the Cloned Code to Your New Repo
Update the local clone's remote to point to your new repo and push the initial code.

1. **Check the Current Remote:**
    - In the terminal (inside recipe-discovery-challenge):
      ```bash
      git remote -v
    - It should show my repo (developer-tii).
2. **Update the Remote:**
    - Set the remote to your new repo: make sure to replace [yourusername] with your name.
      ```bash
      git remote set-url origin https://github.com/[yourusername]/recipe-discovery-solution.git
    - Verify:
      ```bash
      git remote - v
3. **Push the code:**
    - Push the existing code:
      ```bash
      git push origin main
    - Refresh your GitHub repo page to confirm the files (e.g., README.md) are uploaded.
## Step 4: Work on your solution

## Step 5: Share your solution with us
Invite me as a collaborator to your private repo so we can review your work.

1. **Go to your Repo Settings:**
    - Navigate to your repo (e.g., https://github.com/yourusername/recipe-discovery-solution).
    - Click Settings at the top.
2. **Add Me as a Collaborator:**
    - In the left sidebar, click Collaborators (under "Access").
    - Click Add People
    - Enter my GitHub username: developer-tii
    - Set my role to Read (or Write if you're okay with me commenting directly).
    - Click Add.
4. **Notify me:**
    - Send me an email letting me know you have completed the challenge and have invited me as a collaborator. 

## Step 6: Verify your Submission
    - Ensure your repo includes all deliverables:
        - Source Code.
        - Updated README.md with setup instructions and a sample GraphQL query/mutation.
        - Explanation of your design choices (in README or comments).
    - Test your solution locally to confirm it works as expected.