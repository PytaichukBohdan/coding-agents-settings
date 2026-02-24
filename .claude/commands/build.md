---
description: Build the codebase based on the plan
argument-hint: [path-to-plan]
allowed-tools: Read, Write, Edit, Bash, Grep, Glob, Task, TaskCreate, TaskUpdate, TaskList, TaskGet
---

# Build

Follow the `Workflow` to implement the `PATH_TO_PLAN` then `Report` the completed work.

## Variables

PATH_TO_PLAN: $ARGUMENTS

## Instructions

- IMPORTANT: Implement the plan top to bottom, in order. Do not skip any steps. Do not stop in between steps. Complete every step in the plan before stopping.
  - Make your best guess judgement based on the plan, everything will be detailed there.
  - If you have not run any validation commands throughout your implementation, DO NOT STOP until you have validated the work.
  - Your implementation should end with executing the validation commands to validate the work, if there are issues, fix them before stopping.
- Follow the `Spec Status Update Protocol` below to update the plan file in real-time as you work.
- For complex plans with a `## Team Orchestration` section, use Task tools (`TaskCreate`, `TaskUpdate`, `TaskList`, `TaskGet`) to deploy team agents (builder, validator) for assigned tasks. Delegate work to `.claude/agents/team/*.md` agents as defined in the orchestration section.
- **Skill reference**: Follow the `verification-before-completion` skill before claiming completion.

## The Iron Law

```
NO COMPLETION CLAIM WITHOUT VERIFIED EVIDENCE
```

Claiming completion without running validation? Start over.

**No exceptions:**
- Don't claim "should work" - prove it works
- Don't trust previous run results - run again
- Don't skip validation "this once"
- Evidence or it didn't happen

## Spec Status Update Protocol

This is the key mechanism for real-time plan tracking. As you work through the plan, update the spec file itself using the Edit tool so that anyone inspecting the plan can see current progress.

**Status values:** `pending` | `in_progress` | `completed` | `blocked` | `skipped`

**Protocol:**

1. **Before starting a phase/task:**
   - Set `Status:` to `in_progress`
   - Add a timestamp to `Comments:` indicating work has begun

2. **After completing a phase/task:**
   - Check the checkbox `- [x]`
   - Set `Status:` to `completed`
   - Add completion notes to `Comments:` describing what was done

3. **If a phase/task fails or is blocked:**
   - Set `Status:` to `blocked`
   - Add failure details and root cause to `Comments:`

4. **If intentionally skipping a phase/task:**
   - Set `Status:` to `skipped`
   - Add rationale explaining why it was skipped to `Comments:`

**Example - Updating a task status with Edit tool:**

Before:
```markdown
- [ ] Implement authentication module
  - Status: pending
  - Comments:
```

After starting work, use the Edit tool to change it to:
```markdown
- [ ] Implement authentication module
  - Status: in_progress
  - Comments: Started implementation at 2026-02-24T10:30:00
```

After completing, use the Edit tool again:
```markdown
- [x] Implement authentication module
  - Status: completed
  - Comments: Started implementation at 2026-02-24T10:30:00. Completed - added JWT-based auth with refresh tokens, all tests passing.
```

## Verification Gate (MANDATORY)

BEFORE claiming implementation is complete:

1. **IDENTIFY**: What validation commands prove completion?
2. **RUN**: Execute EVERY validation command from the plan (fresh, complete)
3. **READ**: Full output - check exit codes, count failures
4. **VERIFY**: Does ALL output confirm success?
   - If NO: Fix issues, re-run, repeat
   - If YES: Include evidence in report
5. **ONLY THEN**: Claim completion

Skip any step = incomplete implementation. Return to Step 1.

**Evidence required for completion claim:**
- Validation command output (actual output, not "it passed")
- Test results with pass/fail counts
- `git diff --stat` summary

## Red Flags - STOP Implementation

If any of these thoughts occur to you, STOP and reconsider:

- Writing code before reading relevant files
- Skipping validation "because code looks correct"
- "I'll fix the tests later"
- Committing without running checks
- "Just a small change, no need to verify"
- Using "should work" or "probably passes"
- Claiming completion before running commands
- Trusting previous run results

**If any of these apply: STOP. Run validation commands NOW.**

## Common Rationalizations

| Excuse | Reality |
|--------|---------|
| "Code is obviously correct" | Obvious code fails all the time. Verify. |
| "Tests were passing earlier" | Previous runs prove nothing. Run again. |
| "Just a small change" | Small changes break things. Full verification. |
| "I'll test it later" | Later never comes. Test now. |
| "The plan said to do this" | Plans can be wrong. Verify outcomes. |

## Announcement (MANDATORY)

Before starting work, announce:

"I'm using /build to implement the plan at [path]. I will follow the workflow exactly and verify all work before claiming completion."

This creates commitment. Skipping this step = likely to skip other steps.

## Workflow

- If no `PATH_TO_PLAN` is provided, STOP immediately and ask the user to provide it.

  **No exceptions:**
  - Don't infer the plan from conversation
  - Don't create an ad-hoc plan
  - Don't proceed without an explicit path
  - STOP means STOP

- Read the plan at `PATH_TO_PLAN`. Ultrathink about the plan and IMPLEMENT it into the codebase.
  - Implement the entire plan top to bottom before stopping.

- If the plan has a `## Team Orchestration` section, use Task tools to deploy team agents for assigned tasks:
  - Use `TaskCreate` to create tasks for builder/validator agents as defined in the orchestration section
  - Use `TaskList` and `TaskGet` to monitor progress of delegated tasks
  - Use `TaskUpdate` to update coordination status
  - Team agents are defined in `.claude/agents/team/*.md` (builder, validator)

- Update spec status fields as you work using the `Spec Status Update Protocol` above.
- Run validation commands before claiming completion per the `Verification Gate`.

## Report

- Summarize the work you've just done in a concise bullet point list.
- Report the files and total lines changed with `git diff --stat`
- Spec status summary - include a table showing completion state:

| Phase/Task | Assigned Agent | Status |
|------------|---------------|--------|
| [phase/task name] | [self / builder / validator] | completed / blocked / skipped |
| ... | ... | ... |
