---
name: verification-before-completion
description: Enforces evidence-based verification before claiming any task is complete. Use when finishing any implementation, fix, or task that requires proof of completion.
---

# Verification Before Completion

## Overview

Never claim completion without proof. Run the commands. Check the output. Provide evidence.

**Core principle:** A claim without evidence is not a claim—it's a guess.

## The Iron Law

```
NO COMPLETION CLAIM WITHOUT VERIFIED EVIDENCE
```

Saying "it should work" or "it's done"? That's not verification.

**No exceptions:**
- Don't claim based on what you wrote
- Don't trust previous test runs
- Don't assume success
- Evidence or it didn't happen

## The Gate Function

BEFORE claiming any status or expressing satisfaction:

```
┌─────────────────────────────────────────────────────────────────┐
│                        THE GATE FUNCTION                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   1. IDENTIFY  →  What command proves this claim?               │
│       │                                                         │
│       ▼                                                         │
│   2. RUN       →  Execute the FULL command (fresh, complete)    │
│       │                                                         │
│       ▼                                                         │
│   3. READ      →  Full output, check exit code, count failures  │
│       │                                                         │
│       ▼                                                         │
│   4. VERIFY    →  Does output CONFIRM the claim?                │
│       │                                                         │
│       ├── NO  →  Fix the issue. Return to step 2.               │
│       │                                                         │
│       └── YES →  5. CLAIM  →  Make the claim WITH evidence      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘

Skip any step = lying, not verifying
```

### Step 1: IDENTIFY

Ask: "What command would prove my claim?"

| Claim | Verification Command |
|-------|---------------------|
| "Tests pass" | `npm test` / `pytest` / `go test` |
| "It compiles" | `npm run build` / `cargo build` / `go build` |
| "No lint errors" | `npm run lint` / `ruff check` |
| "Types are correct" | `tsc --noEmit` / `mypy` |
| "It works" | Specific command that exercises the feature |

### Step 2: RUN

Execute the command. Not a partial run. Not from memory. Fresh. Complete.

**Requirements:**
- Run the FULL command (not just part of test suite)
- Run it NOW (not "I ran it earlier")
- Capture the output
- Note the exit code

### Step 3: READ

Read the ENTIRE output. Not just the summary. The whole thing.

**Check for:**
- Exit code (0 = success, non-zero = failure)
- Failure counts (0 failures required)
- Warning messages (may indicate problems)
- Error messages (any = not complete)

### Step 4: VERIFY

Does the output ACTUALLY confirm your claim?

**Not verified:**
- "3 tests passed" (but you added 5 tests)
- "Build succeeded with warnings" (warnings may matter)
- "Exit code 0" (but error messages in output)

**Verified:**
- All tests pass, count matches expected
- Build succeeds, no warnings, exit code 0
- Output explicitly confirms the behavior

### Step 5: CLAIM (Only After Verification)

Now you can claim completion. Include the evidence:

```markdown
✅ Implementation complete

Evidence:
- Tests: 15 passed, 0 failed
- Build: Success, exit code 0
- Lint: 0 errors, 0 warnings
```

## Red Flags - STOP Immediately

If any of these thoughts occur to you, STOP:

- "It should work"
- "It's probably fine"
- "I already tested this"
- "The tests were passing earlier"
- "This is obvious"
- "Just a small change, no need to verify"
- "I'll check later"
- About to claim completion without running a command

**If any of these apply: STOP. Run the verification commands NOW.**

## Common Rationalizations

| Excuse | Reality |
|--------|---------|
| "Tests were passing earlier" | Previous runs prove nothing. Run again. |
| "It's the same code" | Environments differ. Verify now. |
| "Just a small change" | Small changes cause big bugs. Verify. |
| "I know it works" | Knowing ≠ proving. Show evidence. |
| "Takes too long to run" | Unverified claims waste more time. |
| "The CI will catch it" | Your job to verify, not CI's. |

## Workflow

When completing any task:

1. **Before claiming done, STOP**
   - Don't say "done" yet
   - Don't say "should work"
   - Don't express satisfaction

2. **Identify verification commands**
   - What would prove this works?
   - What would prove tests pass?
   - What would prove it builds?

3. **Run each command**
   - Full execution
   - Capture output
   - Note exit codes

4. **Read and verify output**
   - Check for failures
   - Check for warnings
   - Confirm expected behavior

5. **Only then, claim completion with evidence**
   - Include command outputs
   - Include test counts
   - Include specific verification results

## Examples

### Example 1: Completing a Feature

❌ **Wrong:**
```
I've implemented the user authentication feature. It should work correctly now.
```

✅ **Right:**
```
I've implemented the user authentication feature.

Verification:
$ npm test
  ✓ user can login with valid credentials
  ✓ user cannot login with invalid password
  ✓ session expires after timeout
  3 passing (0.5s)

$ npm run build
  Build succeeded, exit code 0

$ npm run lint
  No errors or warnings

All verification passed. Feature complete.
```

### Example 2: Fixing a Bug

❌ **Wrong:**
```
Fixed the null pointer exception. The bug should be resolved.
```

✅ **Right:**
```
Fixed the null pointer exception by adding null check.

Verification:
$ go test ./...
  ok  	package/handler	0.015s
  PASS

$ go build ./...
  Build succeeded

Reproducing the bug scenario:
$ curl -X POST /api/endpoint -d '{}'
  {"status": "success"}  # Previously crashed

Bug fix verified. Issue resolved.
```

## Verification Checklist

Before claiming ANY task complete:

- [ ] Identified all verification commands
- [ ] Ran each command (fresh, complete)
- [ ] Read all output (not just summary)
- [ ] Confirmed all checks pass
- [ ] Included evidence in completion claim

If any checkbox is unchecked, DO NOT claim completion.
