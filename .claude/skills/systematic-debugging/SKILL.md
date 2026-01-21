---
name: systematic-debugging
description: Enforces a methodical approach to debugging that prevents trial-and-error fixes. Use when investigating bugs, errors, or unexpected behavior in code.
---

# Systematic Debugging

## Overview

Debug systematically. Understand before fixing. One change at a time.

**Core principle:** A bug fixed without understanding is a bug waiting to return.

## The Iron Law

```
NO FIX WITHOUT UNDERSTANDING THE ROOT CAUSE
```

Made a change that "seems to work"? That's not debugging—that's guessing.

**No exceptions:**
- Don't make random changes hoping something works
- Don't fix symptoms without understanding causes
- Don't claim "fixed" without explaining WHY it was broken
- Understand first, then fix

## The Debugging Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                     SYSTEMATIC DEBUGGING                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   1. REPRODUCE  →  Can you make the bug happen consistently?    │
│       │                                                         │
│       ▼                                                         │
│   2. ISOLATE    →  What is the smallest case that fails?        │
│       │                                                         │
│       ▼                                                         │
│   3. UNDERSTAND →  WHY does it fail? What's the root cause?     │
│       │                                                         │
│       ▼                                                         │
│   4. FIX        →  Make ONE change to address root cause        │
│       │                                                         │
│       ▼                                                         │
│   5. VERIFY     →  Bug gone? No new bugs? Explain the fix.      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘

Skip any step = debugging theater, not debugging
```

### Step 1: REPRODUCE

Before anything else, reproduce the bug reliably.

**Requirements:**
- Document exact steps to trigger the bug
- Bug must happen consistently
- Note the exact error message/behavior
- Record the environment (versions, configs)

**If you can't reproduce it:**
- Gather more information
- Check logs, error reports
- Don't proceed until you can reproduce

### Step 2: ISOLATE

Find the smallest case that exhibits the bug.

**Techniques:**
- Binary search through code changes (git bisect)
- Remove components until bug disappears
- Create minimal reproduction case
- Identify exact input that triggers failure

**Goal:** Narrow down from "somewhere in the app" to "this specific function/line"

### Step 3: UNDERSTAND

This is the critical step most people skip.

**Ask and answer:**
- WHY does this code fail with this input?
- WHAT is the actual vs expected behavior?
- WHERE does the logic go wrong?
- WHEN was this introduced? (git blame/log)

**Form a hypothesis:**
- "The bug occurs because X leads to Y which causes Z"
- Write it down before fixing
- If you can't explain it, you don't understand it

### Step 4: FIX

Make ONE change that addresses the root cause.

**Rules:**
- Only change what's necessary
- Don't "fix" other things while you're here
- Don't add defensive code without understanding
- Keep the change minimal and focused

**Write a test first:**
- Test should fail before fix
- Test should pass after fix
- Test prevents regression

### Step 5: VERIFY

Prove the bug is fixed and nothing else broke.

**Verification checklist:**
- [ ] Original bug no longer reproduces
- [ ] All existing tests pass
- [ ] New test for this bug passes
- [ ] No new warnings or errors
- [ ] Can explain WHY the fix works

## Red Flags - STOP and Restart

If any of these happen, you're not debugging systematically:

- Making changes without reproducing the bug first
- "Let me try this and see if it works"
- Multiple changes at once
- Can't explain why the fix works
- "It works now" but don't know why it was broken
- Copying code from Stack Overflow without understanding
- Adding try/catch to hide errors instead of fixing them

**If any of these apply: STOP. Return to step 1.**

## Common Rationalizations

| Excuse | Reality |
|--------|---------|
| "I'll just try this quick fix" | Quick fixes hide bugs. Understand first. |
| "The fix works, who cares why" | You will care when it breaks again. |
| "No time to investigate properly" | Guessing wastes more time than investigating. |
| "It's probably X" | Probably ≠ definitely. Verify your hypothesis. |
| "I'll clean it up later" | Later never comes. Fix it right now. |
| "This is too complex to understand" | Break it down. Everything can be understood. |

## Workflow

When debugging, follow this exact sequence:

1. **Announce systematic debugging**
   - "I'm debugging systematically. I will understand before fixing."

2. **Reproduce**
   - Document exact reproduction steps
   - Verify bug happens consistently
   - If can't reproduce: gather more info

3. **Isolate**
   - Find minimal reproduction case
   - Narrow down to specific code area
   - Identify exact trigger conditions

4. **Form hypothesis**
   - Write down: "The bug occurs because..."
   - If you can't write this, keep investigating

5. **Write failing test**
   - Test that captures the bug
   - Run it, watch it fail

6. **Make ONE fix**
   - Address root cause
   - Minimal change

7. **Verify**
   - Test passes
   - All other tests pass
   - Can explain the fix

8. **Document**
   - What was the bug?
   - What caused it?
   - How was it fixed?

## Examples

### Example 1: Null Pointer Exception

**Wrong approach:**
```
Got null pointer exception. Let me add a null check and see if that helps.
```

**Right approach:**
```
1. REPRODUCE: POST to /api/users with empty body causes NPE

2. ISOLATE: Traced to UserService.create() line 45

3. UNDERSTAND: The bug occurs because:
   - Request body is parsed as null when empty
   - UserService assumes body is never null
   - Calls user.getName() on null object

4. FIX: Add input validation at controller level
   - Reject empty body with 400 Bad Request
   - (NOT: add null check that hides the problem)

5. VERIFY:
   - Empty body now returns 400 with clear error
   - Valid body still works
   - Added test: test_empty_body_returns_400
```

### Example 2: Intermittent Test Failure

**Wrong approach:**
```
Test fails sometimes. Let me add a retry or increase timeout.
```

**Right approach:**
```
1. REPRODUCE: Test fails ~20% of runs on CI
   - Ran locally 50 times, failed 12 times

2. ISOLATE:
   - Failure only happens when tests run in parallel
   - Specific to tests using shared database

3. UNDERSTAND: The bug occurs because:
   - Two tests use same user ID "test-user-1"
   - Test A creates user, Test B deletes user
   - When run in parallel, race condition

4. FIX: Each test uses unique user ID
   - Use UUID for test fixtures
   - Clean up after each test

5. VERIFY:
   - Ran 100 parallel executions: 0 failures
   - Test isolation confirmed
```

## Debugging Tools Reference

| Tool | Use Case |
|------|----------|
| `git bisect` | Find which commit introduced bug |
| `git blame` | Find who wrote problematic code and when |
| `print/console.log` | Trace execution flow (remove after!) |
| Debugger (breakpoints) | Inspect state at specific points |
| `git diff` | Compare working vs broken versions |
| Logs | Find errors, trace requests |

## Verification Checklist

Before claiming a bug is fixed:

- [ ] Could reproduce the bug consistently
- [ ] Identified the minimal reproduction case
- [ ] Can explain WHY the bug occurred
- [ ] Made ONE focused change
- [ ] Wrote a test that would catch regression
- [ ] All tests pass
- [ ] Can explain WHY the fix works

If any checkbox is unchecked, you're not done debugging.
