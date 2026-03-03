# Claude Code Agent Toolkit

Plug-and-play configuration for Claude Code -- skills, agents, commands, hooks, and team orchestration out of the box.

| Category | Count |
|----------|-------|
| Skills | 10 |
| Agents | 9 |
| Commands | 14+ |
| Hooks | 13+ |
| Output Styles | 11 |

---

## Quick Start

```bash
# 1. Clone the repo
git clone https://github.com/<your-org>/coding-agents-settings.git

# 2. Copy the .claude/ directory into your project
cp -r coding-agents-settings/.claude/ /path/to/your-project/.claude/

# 3. Open your project in Claude Code and orient yourself
/prime

# 4. Create your first implementation plan
/plan_w_team implement user auth

# 5. Execute the plan
/build specs/implement-user-auth.md
```

That's it. You now have a full planning-and-building workflow, 10 reusable skills, 9 specialized agents, and a hook system that logs everything.

---

## Core Workflow -- Plan then Build

This is the primary way to get things done. Think first, then do.

### Step 1: Plan with `/plan_w_team`

`/plan_w_team` creates a detailed implementation spec and saves it to the `specs/` directory. It does **not** write any code. It analyzes your requirements, designs a solution, assembles a team, and defines step-by-step tasks with dependencies.

```
/plan_w_team add dark mode support
```

The output is a spec file (`specs/add-dark-mode-support.md`) that contains:

- **Task description and objective** -- what needs to happen and why
- **Relevant files** -- which parts of the codebase are involved
- **Implementation phases** -- with checkboxes for tracking progress
- **Team orchestration** -- which agents will do the work
- **Step-by-step tasks** -- with IDs, dependencies, and assignments
- **Acceptance criteria** -- measurable conditions for "done"
- **Validation commands** -- concrete commands to prove it works

### Step 2: Build with `/build`

`/build` takes a spec file and implements it top-to-bottom. It updates the spec file in real-time so you can see progress at any moment.

```
/build specs/add-dark-mode-support.md
```

As work progresses, statuses update in the spec:

```markdown
- [ ] Setup theme variables
  - Status: pending          -->  in_progress  -->  completed
  - Comments:                     Started...        Done - added CSS vars
```

Before claiming completion, `/build` enforces **The Iron Law**:

> NO COMPLETION CLAIM WITHOUT VERIFIED EVIDENCE

Every validation command in the spec must pass. No "should work" -- only proof.

### Why This Matters

The plan is the blueprint. The build is the execution. Together they prevent the most common failure mode in AI-assisted coding: jumping straight into implementation without thinking it through.

---

## Team Orchestration

`/plan_w_team` creates plans that use team agents to divide and conquer work.

### Team Agents

| Agent | File | Role |
|-------|------|------|
| **Builder** | `.claude/agents/team/builder.md` | Executes ONE task at a time -- writes code, creates files, implements features |
| **Validator** | `.claude/agents/team/validator.md` | Read-only agent that verifies work meets acceptance criteria |

### How It Works

1. **TaskCreate** -- the lead creates tasks from the plan
2. **TaskUpdate** -- tasks get assigned to builders, dependencies are set
3. **Builders execute** -- each builder focuses on one task, marks it completed
4. **Validators verify** -- read-only checks confirm the work is correct
5. **Parallel execution** -- multiple builders can run simultaneously on independent tasks

Tasks can block each other. Task 2 won't start until Task 1 is done:

```
Task 1: Setup database schema     --> no dependencies
Task 2: Implement API endpoints   --> blocked by Task 1
Task 3: Write integration tests   --> blocked by Task 2
Task 4: Final validation          --> blocked by Tasks 1, 2, 3
```

---

## Skills

Skills are reusable capabilities that Claude loads on-demand. Each skill is a directory inside `.claude/skills/` containing a `SKILL.md` file -- like an onboarding guide that teaches Claude a specific workflow.

Skills use **progressive disclosure**:
1. **Metadata** (always loaded) -- name and description in YAML frontmatter
2. **Instructions** (loaded when triggered) -- the main body of SKILL.md
3. **Resources** (loaded as needed) -- additional files, scripts, templates

| Skill | Directory | Description |
|-------|-----------|-------------|
| Meta-Skill | `meta-skill/` | Creates new skills (meta!) |
| Test-Driven Development | `test-driven-development/` | Enforces TDD: Red -> Green -> Refactor |
| Systematic Debugging | `systematic-debugging/` | Prevents trial-and-error: Reproduce -> Isolate -> Understand -> Fix -> Verify |
| Verification Before Completion | `verification-before-completion/` | No completion without proof |
| Playwright Bowser | `playwright-bowser/` | Headless browser automation with Playwright CLI |
| Worktree Manager | `worktree-manager-skill/` | Comprehensive git worktree management |
| Just | `just/` | Task runner recipes (alternative to make) |
| Video Processor | `video-processor/` | Video conversion + Whisper transcription |
| Unity Assets | `unity-assets/` | Unity game asset extraction and conversion |
| Create Worktree | `create-worktree-skill/` | Quick worktree creation |

---

## Agents

Agents are specialized sub-processes defined as Markdown files in `.claude/agents/`. Each one has a specific role, set of tools, and model.

| Agent | File | Role | Model |
|-------|------|------|-------|
| Meta-Agent | `meta-agent.md` | Creates new agents (meta!) | opus |
| Docs Scraper | `docs-scraper.md` | Fetches documentation as markdown | haiku |
| Scout Report | `scout-report-suggest.md` | Read-only codebase analysis | haiku |
| Scout Report (Fast) | `scout-report-suggest-fast.md` | Fast codebase analysis | haiku |
| Bowser QA | `bowser-qa-agent.md` | UI testing with screenshots | opus |
| Playwright Bowser | `playwright-bowser-agent.md` | Browser automation | opus |
| Create Worktree | `create_worktree_subagent.md` | Worktree creation specialist | sonnet |
| Team Builder | `team/builder.md` | Code implementation (team) | opus |
| Team Validator | `team/validator.md` | Work verification (team) | opus |

---

## Commands (Slash Commands)

Commands are custom prompts located in `.claude/commands/`. Trigger them with `/name` and pass arguments using `$1`, `$2`, or `$ARGUMENTS`.

| Command | What It Does |
|---------|-------------|
| `/plan_w_team` | Create an implementation plan with team orchestration |
| `/build` | Execute a plan spec into the codebase |
| `/prime` | Quick codebase orientation (reads files, summarizes) |
| `/start` | Start the observability system |
| `/load_ai_docs` | Cache documentation locally for agent context |
| `/ui-review` | Parallel QA testing -- discovers user stories, fans out browser agents, reports pass/fail |
| `/create_worktree_prompt` | Create an isolated git worktree |
| `/list_worktrees_prompt` | List all active worktrees |
| `/remove_worktree_prompt` | Remove a worktree |
| `/t_metaprompt_workflow` | Create a new command (meta!) |
| `/convert_paths_absolute` | Fix relative paths in settings |

**Subdirectory commands:**

| Command | What It Does |
|---------|-------------|
| `/bench/find_and_summarize` | Find and summarize codebase patterns |
| `/bench/load_ai_docs` | Load AI documentation for benchmarks |
| `/bench/plan_new_feature` | Plan a new feature (benchmark) |
| `/bowser/amazon-add-to-cart` | Browser automation: Amazon cart |
| `/bowser/blog-summarizer` | Browser automation: blog summarization |
| `/bowser/hop-automate` | Browser automation: HOP workflow |

---

## Meta-Tools -- Creating Your Own

This is where it gets interesting. The toolkit includes three meta-capabilities that create more of their own kind:

### 1. Meta-Skill (`.claude/skills/meta-skill/`)

Creates new skills. Ask Claude to create a skill and it generates a complete `SKILL.md` with frontmatter, instructions, examples, and supporting files.

```
You: "Create a skill for linting Dockerfiles"
--> Generates .claude/skills/dockerfile-lint/SKILL.md
```

The meta-skill reads Anthropic's latest skill documentation, asks clarifying questions, then follows the progressive disclosure pattern to build a well-structured skill.

### 2. Meta-Agent (`.claude/agents/meta-agent.md`)

Creates new agents. Describe what you need and it generates a complete agent definition with the right tools, model, and color.

```
You: "Create an agent that reviews SQL queries for performance"
--> Generates .claude/agents/sql-performance-reviewer.md
```

It scrapes the latest Claude Code sub-agent documentation, selects the minimal toolset, and writes a focused system prompt.

### 3. Meta-Command (`/t_metaprompt_workflow`)

Creates new slash commands. Describe what you want and it generates a properly formatted command file.

```
/t_metaprompt_workflow create a command to generate API documentation from code comments
--> Generates .claude/commands/generate-api-docs.md
```

It fetches the latest Anthropic docs for proper slash command format, then builds a command with variables, workflow, and report sections.

**The pattern:** skills create skills, agents create agents, commands create commands. The toolkit grows itself.

---

## Hooks

Hooks are Python scripts that run on lifecycle events. They live in `.claude/hooks/` and are configured in `.claude/settings.json`.

**Lifecycle events covered:**

| Event | Hook | Purpose |
|-------|------|---------|
| PreToolUse | `pre_tool_use.py` | Security gate -- blocks dangerous commands like `rm -rf` |
| PostToolUse | `post_tool_use.py` | Post-execution logging and processing |
| PostToolUseFailure | `post_tool_use_failure.py` | Failure tracking |
| SessionStart | `session_start.py` | Session initialization |
| SessionEnd | `session_end.py` | Session cleanup and logging |
| UserPromptSubmit | `user_prompt_submit.py` | Prompt logging and agent naming |
| Stop | `stop.py` | Stop event handling |
| PreCompact | `pre_compact.py` | Context compaction preprocessing |
| SubagentStart | `subagent_start.py` | Sub-agent lifecycle tracking |
| SubagentStop | `subagent_stop.py` | Sub-agent completion handling |
| Notification | `notification.py` | Desktop notifications |
| PermissionRequest | `permission_request.py` | Permission event logging |

Every event also fires `send_event.py` for observability -- all tool uses, sessions, and sub-agents are logged to `.claude/sessions/`.

**Code quality validators** (in `.claude/hooks/validators/`):
- `ruff_validator.py` -- lints Python code with Ruff on every edit
- `ty_validator.py` -- type-checks Python code with ty on every edit
- `validate_new_file.py` -- validates new file creation
- `validate_file_contains.py` -- validates file content requirements

---

## Output Styles

11 output formatting styles in `.claude/output-styles/` that control how Claude formats responses:

| Style | File |
|-------|------|
| Bullet Points | `bullet-points.md` |
| GenUI | `genui.md` |
| HTML Structured | `html-structured.md` |
| Markdown Focused | `markdown-focused.md` |
| Observable Tools + Diffs | `observable-tools-diffs.md` |
| Observable Tools + Diffs + TTS | `observable-tools-diffs-tts.md` |
| Table Based | `table-based.md` |
| TTS Summary Base | `tts-summary-base.md` |
| TTS Summary | `tts-summary.md` |
| Ultra Concise | `ultra-concise.md` |
| YAML Structured | `yaml-structured.md` |

---

## Discipline Skills

Three skills enforce engineering methodology across all work. These are automatically referenced by `/build`:

1. **`verification-before-completion`** -- you must prove your work is done with actual command output, not "it should work"
2. **`systematic-debugging`** -- when investigating bugs, you must Reproduce -> Isolate -> Understand -> Fix -> Verify. No trial-and-error.
3. **`test-driven-development`** -- when implementing features, you must write the failing test first: Red -> Green -> Refactor

These skills transform Claude from "write code and hope" into "prove it works."

---

## Project Structure

```
.claude/
├── agents/              # Sub-agent definitions
│   ├── team/            # Team agents (builder, validator)
│   ├── meta-agent.md
│   ├── docs-scraper.md
│   ├── scout-report-suggest.md
│   ├── scout-report-suggest-fast.md
│   ├── bowser-qa-agent.md
│   ├── playwright-bowser-agent.md
│   └── create_worktree_subagent.md
├── commands/            # Custom slash commands
│   ├── bench/           # Benchmark commands
│   ├── bowser/          # Browser automation commands
│   ├── plan_w_team.md
│   ├── build.md
│   ├── prime.md
│   └── ...
├── hooks/               # Lifecycle event hooks
│   ├── validators/      # Code quality validators (ruff, ty)
│   ├── utils/           # Hook utilities
│   ├── pre_tool_use.py
│   ├── session_start.py
│   └── ...
├── skills/              # Reusable capabilities
│   ├── meta-skill/
│   ├── test-driven-development/
│   ├── systematic-debugging/
│   ├── verification-before-completion/
│   ├── playwright-bowser/
│   └── ...
├── output-styles/       # Output formatting (11 styles)
├── status_lines/        # Status line generators
└── settings.json        # Main configuration
```

---

## Examples

### 1. Plan and build a feature

```
/plan_w_team add a REST API endpoint for user profiles
```

Review the generated spec, then execute it:

```
/build specs/add-rest-api-user-profiles.md
```

### 2. Create a new skill

```
You: "Create a skill for linting Dockerfiles"
```

The meta-skill generates `.claude/skills/dockerfile-lint/SKILL.md` with instructions, workflow, and examples.

### 3. Create a new agent

```
You: "Create an agent that reviews database migrations"
```

The meta-agent generates `.claude/agents/db-migration-reviewer.md` with the right tools, model, and system prompt.

### 4. Create a new command

```
/t_metaprompt_workflow create a command to generate API documentation from code comments
```

Generates `.claude/commands/generate-api-docs.md` with variables, workflow, and report format.

### 5. QA test a web app

```
/ui-review
```

This discovers user stories in `ai_review/user_stories/*.yaml`, launches parallel browser agents (one per story), and returns a pass/fail report with screenshots saved to `screenshots/bowser-qa/`.

---

## Configuration

The main configuration lives in `.claude/settings.json`. Key settings:

- **Permissions** -- allowed and denied shell commands (e.g., `rm -rf` is blocked)
- **Hooks** -- all lifecycle hooks are wired up here
- **Status line** -- custom status line generator
- **Team mode** -- `in-process` for agent teams

You'll need a `.env` file in your project root with your API key:

```
ANTHROPIC_API_KEY=sk-ant-...
```

---

## Links

- [Claude Code Documentation](https://docs.anthropic.com/en/docs/claude-code)
- [Claude Code Skills Guide](https://docs.anthropic.com/en/docs/claude-code/skills)
- [Claude Code Sub-Agents](https://docs.anthropic.com/en/docs/claude-code/sub-agents)
- [Claude Code Slash Commands](https://docs.anthropic.com/en/docs/claude-code/slash-commands)
