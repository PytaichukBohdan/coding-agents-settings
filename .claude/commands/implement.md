---
description: Use when you have a plan file ready and need to implement it into the codebase
argument-hint: [path-to-plan]
---

# Implement

Follow the `Workflow` to implement the `PATH_TO_PLAN` then `Report` the completed work.

## Instructions

- IMPORTANT: Implement the plan top to bottom, in order. Do not skip any steps. Do not stop in between steps. Complete every step in the plan before stopping.
  - Make your best guess judgement based on the plan, everything will be detailed there.
  - If you have not run any validation commands throughout your implementation, DO NOT STOP until you have validated the work.
  - Your implementation should end with executing the validation commands to validate the work, if there are issues, fix them before stopping.

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

## Verification Gate (MANDATORY)

BEFORE claiming implementation is complete:

1. **IDENTIFY**: What validation commands prove completion?
2. **RUN**: Execute EVERY validation command (fresh, complete)
3. **READ**: Full output - check exit codes, count failures
4. **VERIFY**: Does ALL output confirm success?
   - If NO: Fix issues, re-run, repeat
   - If YES: Include evidence in report
5. **ONLY THEN**: Claim completion

Skip any step = incomplete implementation. Return to Step 1.

**Evidence required for completion claim:**
- Validation command output (actual output, not "it passed")
- Test results with pass/fail counts
- Git diff summary

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

"I'm using /implement to implement the plan at [path]. I will follow the workflow exactly and verify all work before claiming completion."

This creates commitment. Skipping this step = likely to skip other steps.

## Variables

PATH_TO_PLAN: $ARGUMENTS

## Workflow

- If no `PATH_TO_PLAN` is provided, STOP immediately and ask the user to provide it.

  **No exceptions:**
  - Don't infer the plan from conversation
  - Don't create an ad-hoc plan
  - Don't proceed without an explicit path
  - STOP means STOP

- Read the plan at `PATH_TO_PLAN`. Ultrathink about the plan and IMPLEMENT it into the codebase.
  - Implement the entire plan top to bottom before stopping.

## Report

- Summarize the work you've just done in a concise bullet point list.
- Report the files and total lines changed with `git diff --stat`
