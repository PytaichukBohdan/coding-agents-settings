# Plan: Create Comprehensive README

## Task Description
Create a README.md for the `coding-agents-settings` repository that explains all functionality in a clear, approachable way. The README should make a newcomer understand the entire system — from the core philosophy of code agents to practical usage of every component. Primary focus on **Plan with Team** and **Build** commands, meta-capabilities (meta-skill, meta-agent, meta-command), and simple usage examples.

## Objective
A single `README.md` file at the project root that serves as a complete onboarding guide. A person reading it should understand:
- What this repo is (a Claude Code agent configuration toolkit)
- The Plan → Build workflow
- How skills, agents, commands, and hooks work
- How to create new skills, agents, and commands using meta-tools
- Practical examples they can copy-paste and run

## Problem Statement
The repo has 10+ skills, 9 agents, 14+ commands, 13+ hooks, and multiple meta-tools — but no README to explain any of it. A new user has zero entry point to understand the system.

## Solution Approach
Create a structured README with progressive disclosure: start with a quick overview and the most impactful workflow (Plan → Build), then go deeper into each subsystem. Use simple, real-world examples throughout.

## Relevant Files

- `.claude/commands/plan_w_team.md` — Plan with Team command (core focus)
- `.claude/commands/build.md` — Build command (core focus)
- `.claude/commands/t_metaprompt_workflow.md` — Meta-command for creating new commands
- `.claude/agents/meta-agent.md` — Meta-agent for creating new agents
- `.claude/skills/meta-skill/SKILL.md` — Meta-skill for creating new skills
- `.claude/agents/team/builder.md` — Builder team agent
- `.claude/agents/team/validator.md` — Validator team agent
- `.claude/agents/` — All agent definitions
- `.claude/skills/` — All skill definitions
- `.claude/commands/` — All command definitions
- `.claude/hooks/` — All hook scripts
- `.claude/settings.json` — Main configuration
- `.claude/output-styles/` — Output formatting styles
- `justfile` — Task runner recipes
- `ai_docs/README.md` — Documentation cache index
- `ai_review/user_stories/` — QA user stories

### New Files
- `README.md` — The main README at project root

## Implementation Phases

- [x] **Phase 1: Research & Outline** — Finalize README structure and gather all key details
  - Status: completed
  - Comments: Started orchestration at 2026-03-02. Completed - builder agent explored codebase and gathered all details.

- [x] **Phase 2: Core Content** — Write the README with all sections, examples, and formatting
  - Status: completed
  - Comments: README.md written by readme-writer agent with all required sections.

- [x] **Phase 3: Validation** — Verify all file paths, command names, and examples are accurate
  - Status: completed
  - Comments: Deploying readme-validator agent at 2026-03-02. Completed - validator found 3 incorrect worktree command names, builder fixed them.

## Team Orchestration

- You operate as the team lead and orchestrate the team to execute the plan.
- IMPORTANT: You NEVER operate directly on the codebase. You use `Task` and `Task*` tools to deploy team members.

### Team Members

- Builder
  - Name: readme-writer
  - Role: Write the complete README.md file based on the detailed content specification below
  - Agent Type: general-purpose
  - Resume: true

- Validator
  - Name: readme-validator
  - Role: Verify all file paths, command syntax, and examples in the README are accurate
  - Agent Type: validator
  - Resume: false

## Step by Step Tasks

### 1. Write the README.md
- **Task ID**: write-readme
- **Depends On**: none
- **Assigned To**: readme-writer
- **Agent Type**: general-purpose
- **Parallel**: false
- Status: completed
- Comments: Deploying readme-writer agent at 2026-03-02. Completed - README.md created at project root.
- Create `README.md` at project root following the exact Content Specification below
- Ensure all sections are written in clear, approachable language
- Use real file paths from this repository
- Keep examples simple and copy-paste ready

#### Content Specification for README.md

The README must follow this exact structure. Write it in Russian since the user communicates in Russian (actually, write in English — it's a public repo, but keep the tone friendly and practical):

---

**HEADER**:
- Title: something catchy like "Claude Code Agent Toolkit" or "Coding Agents Settings"
- One-liner: Plug-and-play configuration for Claude Code — skills, agents, commands, hooks, and team orchestration out of the box.
- A "What's Inside" quick summary table showing counts: 10 Skills, 9 Agents, 14+ Commands, 13+ Hooks, 11 Output Styles

**SECTION: Quick Start**
- Clone the repo
- Copy `.claude/` directory into your project
- Run `/prime` to understand your codebase
- Run `/plan_w_team implement user auth` to create your first plan
- Run `/build specs/implement-user-auth.md` to execute it

**SECTION: Core Workflow — Plan → Build**
This is the PRIMARY focus. Explain the two-step workflow:

1. `/plan_w_team` — Creates a detailed implementation spec in `specs/`. Does NOT write any code. Analyzes requirements, designs solution, defines team members and step-by-step tasks with dependencies.
   - Show a simple example: `/plan_w_team add dark mode support`
   - Explain what the output looks like (a spec file with phases, team members, tasks, acceptance criteria, validation commands)

2. `/build specs/<plan>.md` — Takes the spec and implements it top-to-bottom. Updates the spec file in real-time with status tracking. Enforces "The Iron Law" — no completion claim without verified evidence.
   - Show how statuses update: pending → in_progress → completed
   - Mention the Verification Gate

Explain how together they create a "think first, then do" workflow where the plan is the blueprint and build is the execution.

**SECTION: Team Orchestration**
Explain how /plan_w_team creates plans that use team agents:
- **Builder** (`.claude/agents/team/builder.md`) — executes ONE task at a time, writes code
- **Validator** (`.claude/agents/team/validator.md`) — read-only, verifies work meets criteria
- Task management: TaskCreate → TaskUpdate → TaskList
- Dependencies: tasks can block each other
- Parallel execution: multiple builders can run simultaneously

**SECTION: Skills**
Explain what skills are:
- A directory with a `SKILL.md` file
- Like an "onboarding guide" that Claude loads on-demand
- Progressive disclosure: metadata → instructions → resources
- Located in `.claude/skills/`

List ALL skills with one-line descriptions:
| Skill | Description |
|-------|-------------|
| meta-skill | Creates new skills (meta!) |
| test-driven-development | Enforces TDD: Red → Green → Refactor |
| systematic-debugging | Prevents trial-and-error: Reproduce → Isolate → Understand → Fix → Verify |
| verification-before-completion | No completion without proof |
| playwright-bowser | Headless browser automation |
| worktree-manager-skill | Parallel development with isolated worktrees |
| just | Task runner recipes (alternative to make) |
| video-processor | Video conversion + Whisper transcription |
| unity-assets | Unity game asset extraction |
| create-worktree-skill | Quick worktree creation |

**SECTION: Agents**
Explain what agents are:
- Markdown files in `.claude/agents/`
- Specialized sub-processes with specific tools and capabilities
- Each has a model, color, and tool restrictions

List ALL agents with roles:
| Agent | Role | Model |
|-------|------|-------|
| meta-agent | Creates new agents (meta!) | opus |
| docs-scraper | Fetches documentation as markdown | haiku |
| scout-report-suggest | Read-only codebase analysis | haiku |
| scout-report-suggest-fast | Fast codebase analysis | haiku |
| bowser-qa-agent | UI testing with screenshots | opus |
| playwright-bowser-agent | Browser automation | opus |
| create-worktree-subagent | Worktree creation specialist | sonnet |
| team/builder | Code implementation (team) | opus |
| team/validator | Work verification (team) | opus |

**SECTION: Commands (Slash Commands)**
Explain commands = custom prompts triggered with `/name`:
- Located in `.claude/commands/`
- Can accept arguments (`$1`, `$2`, `$ARGUMENTS`)
- Have allowed tools, description, model

List key commands:
| Command | What It Does |
|---------|-------------|
| `/plan_w_team` | Create implementation plan with team orchestration |
| `/build` | Execute a plan into the codebase |
| `/prime` | Quick codebase orientation |
| `/start` | Start the observability system |
| `/load_ai_docs` | Cache documentation locally |
| `/ui-review` | Parallel QA testing with screenshots |
| `/create_worktree` | Create isolated worktree |
| `/list_worktrees` | List all worktrees |
| `/remove_worktree` | Remove a worktree |
| `/t_metaprompt_workflow` | Create a new command (meta!) |
| `/convert_paths_absolute` | Fix relative paths in settings |

**SECTION: Meta-Tools — Creating Your Own**
This is KEY. Explain the three meta-capabilities:

1. **Meta-Skill** (`/meta-skill` or the skill at `.claude/skills/meta-skill/`) — Creates new skills
   - Example: "Create a skill for reviewing Python code" → generates `.claude/skills/python-code-review/SKILL.md`
   - Follows progressive disclosure pattern

2. **Meta-Agent** (`.claude/agents/meta-agent.md`) — Creates new agents
   - Example: "Create an agent that reviews database migrations" → generates `.claude/agents/db-migration-reviewer.md`
   - Auto-selects tools, model, color

3. **Meta-Command** (`/t_metaprompt_workflow`) — Creates new commands
   - Example: `/t_metaprompt_workflow create a command to run database migrations` → generates `.claude/commands/run-migrations.md`
   - Fetches latest Anthropic docs for proper format

Show the "meta" pattern: each tool creates more of its kind. Skills create skills. Agents create agents. Commands create commands.

**SECTION: Hooks**
Brief explanation:
- Python scripts that run on lifecycle events
- Located in `.claude/hooks/`
- Events: PreToolUse, PostToolUse, SessionStart, SessionEnd, etc.
- Security: `pre_tool_use.py` blocks dangerous `rm -rf`
- Validation: `ruff_validator.py` and `ty_validator.py` lint Python on every edit
- Logging: Every tool use, session, subagent logged to `.claude/sessions/`

**SECTION: Output Styles**
Brief: 11 output formatting styles in `.claude/output-styles/`. List them.

**SECTION: Discipline Skills (Special Category)**
Highlight the three "discipline" skills that enforce methodology:
1. `verification-before-completion` — Must prove work is done
2. `systematic-debugging` — Must understand root cause before fixing
3. `test-driven-development` — Must write failing test before code

These are referenced by `/build` automatically.

**SECTION: Project Structure**
Show the directory tree:
```
.claude/
├── agents/           # Sub-agent definitions
│   └── team/         # Team agents (builder, validator)
├── commands/         # Custom slash commands
│   ├── bench/        # Benchmark commands
│   └── bowser/       # Browser automation commands
├── hooks/            # Lifecycle event hooks
│   ├── validators/   # Code quality validators
│   └── utils/        # Hook utilities
├── skills/           # Reusable capabilities
├── output-styles/    # Output formatting
├── status_lines/     # Status line generators
└── settings.json     # Main configuration
```

**SECTION: Examples**
3-4 practical examples:

1. **Plan and build a feature**:
```
/plan_w_team add a REST API endpoint for user profiles
/build specs/add-rest-api-user-profiles.md
```

2. **Create a new skill**:
```
Ask Claude: "Create a skill for linting Dockerfiles"
→ meta-skill generates .claude/skills/dockerfile-lint/SKILL.md
```

3. **Create a new agent**:
```
Ask Claude: "Create an agent that reviews SQL queries for performance"
→ meta-agent generates .claude/agents/sql-performance-reviewer.md
```

4. **Create a new command**:
```
/t_metaprompt_workflow create a command to generate API documentation from code comments
→ generates .claude/commands/generate-api-docs.md
```

5. **QA test a web app**:
```
/ui-review
→ discovers user stories in ai_review/user_stories/
→ launches parallel browser agents
→ returns pass/fail report with screenshots
```

**FOOTER**:
- Link to Claude Code docs: https://docs.anthropic.com/en/docs/claude-code
- Note about `.env` file needed for ANTHROPIC_API_KEY

---

### 2. Validate the README
- **Task ID**: validate-readme
- **Depends On**: write-readme
- **Assigned To**: readme-validator
- **Agent Type**: validator
- **Parallel**: false
- Status: completed
- Comments: Deploying readme-validator agent at 2026-03-02. Completed - all file paths verified, 3 command names fixed.
- Verify every file path mentioned in README actually exists
- Verify command names match actual files in `.claude/commands/`
- Verify skill names match actual directories in `.claude/skills/`
- Verify agent names match actual files in `.claude/agents/`
- Check that directory tree is accurate
- Verify the quick start instructions make sense

## Acceptance Criteria
- [x] README.md exists at project root
- [x] All file paths in README are valid
- [x] All command names match actual command files
- [x] All skill names match actual skill directories
- [x] All agent names match actual agent files
- [x] Examples are simple and practical
- [x] Plan → Build workflow is clearly explained as the primary feature
- [x] Meta-tools section explains all three meta-capabilities
- [x] Directory structure is accurate

## Validation Commands
- `test -f README.md && echo "README exists"` — Verify file exists
- `wc -l README.md` — Check reasonable length (should be 200-500 lines)
- `git diff --stat` — Show what changed

## Notes
- Write in English (public repo)
- Keep tone friendly, practical, not academic
- Prioritize "show, don't tell" — examples over explanations
- No need for badges or shields — keep it clean

**Available discipline skills** (use during implementation):
- `verification-before-completion` - for evidence-based verification gates
