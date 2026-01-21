---
name: test-driven-development
description: Enforces the test-driven development methodology when implementing features or bugfixes. Use when writing any new code, fixing bugs, or implementing features that need tests.
---

# Test-Driven Development (TDD)

## Overview

Write the test first. Watch it fail. Write minimal code to pass. Refactor.

**Core principle:** If you didn't watch the test fail, you don't know if it tests the right thing.

## The Iron Law

```
NO PRODUCTION CODE WITHOUT A FAILING TEST FIRST
```

Write code before the test? Delete it. Start over.

**No exceptions:**
- Don't keep it as "reference"
- Don't "adapt" it while writing tests
- Don't claim "I'll add tests after"
- Delete means DELETE

## Red-Green-Refactor Cycle

### The TDD Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                                                                 │
│   ┌───────┐      ┌───────┐      ┌──────────┐                   │
│   │  RED  │ ───► │ GREEN │ ───► │ REFACTOR │ ───► (repeat)     │
│   └───────┘      └───────┘      └──────────┘                   │
│       │              │               │                          │
│       ▼              ▼               ▼                          │
│   Write a        Write the      Clean up code                   │
│   failing        minimum        while tests                     │
│   test           code to pass   stay green                      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Step 1: RED - Write a Failing Test

1. **Understand the requirement** - What behavior should the code have?
2. **Write ONE test** - Test the smallest piece of functionality
3. **Run the test** - WATCH IT FAIL
4. **Verify failure reason** - Test should fail for the RIGHT reason (missing implementation, not syntax error)

**Critical:** If the test passes immediately, something is wrong:
- Test may not be testing what you think
- Code may already exist
- Test may have a bug

### Step 2: GREEN - Make it Pass

1. **Write minimal code** - Just enough to pass the test
2. **Don't optimize** - That's what refactor is for
3. **Don't add features** - Only what the test requires
4. **Run the test** - WATCH IT PASS

**Critical:** If you're writing more code than the test requires, STOP.

### Step 3: REFACTOR - Clean Up

1. **Tests are green** - Only refactor when tests pass
2. **Improve code structure** - Remove duplication, improve names
3. **Keep tests green** - Run after each change
4. **Don't add functionality** - That's a new RED cycle

**Critical:** If tests fail during refactor, you changed behavior. Undo and try again.

## Red Flags - STOP and Start Over

If any of these happen, DELETE your code and restart with TDD:

- Writing production code before a failing test
- Test passes immediately without new code
- Rationalizing "just this once"
- "This is different because..."
- "Too simple to test"
- "I'll test after"
- "Already manually tested"
- Writing more than minimal code to pass

**All of these mean: Delete code. Start over with TDD.**

## Common Rationalizations

| Excuse | Reality |
|--------|---------|
| "Too simple to test" | Simple code breaks. Test takes 30 seconds. |
| "I'll test after" | Tests passing immediately prove nothing about your code. |
| "Already manually tested" | Ad-hoc ≠ systematic. No record, can't re-run. |
| "This is different" | No it's not. TDD applies to ALL production code. |
| "I know it works" | "Know" ≠ "Prove". Write the test. |
| "Tests slow me down" | Bugs slow you down more. Tests save time. |

## Workflow

When using TDD, follow this exact sequence:

1. **Announce TDD** - "I'm using TDD. I will write a failing test first."

2. **RED Phase**
   - Write ONE test for ONE small behavior
   - Run the test
   - Verify it FAILS for the expected reason
   - If it passes: investigate why, don't proceed

3. **GREEN Phase**
   - Write MINIMAL code to pass the test
   - No extra features, no optimization
   - Run the test
   - Verify it PASSES

4. **REFACTOR Phase**
   - Improve code structure (if needed)
   - Run tests after each change
   - All tests must stay green

5. **Repeat**
   - Go back to RED for next behavior
   - Continue until feature is complete

## Examples

### Example 1: Adding a Validation Function

User request: "Add email validation to the user model"

Using TDD:

**RED:**
```python
# test_user.py
def test_email_validation_rejects_invalid_email():
    user = User(email="not-an-email")
    assert user.is_valid() == False
```
Run test → FAILS (no validation logic exists)

**GREEN:**
```python
# user.py
def is_valid(self):
    return "@" in self.email
```
Run test → PASSES

**REFACTOR:**
- Add proper email regex
- Keep test green
- Add more tests for edge cases

### Example 2: Fixing a Bug

User request: "Fix bug where negative numbers crash the calculator"

Using TDD:

**RED:**
```python
def test_calculator_handles_negative_numbers():
    result = calculate(-5, 10)
    assert result == 5
```
Run test → FAILS (crashes as expected)

**GREEN:**
```python
def calculate(a, b):
    return abs(a) + abs(b)  # Minimal fix
```
Run test → PASSES

**REFACTOR:**
- Review if abs() is correct behavior
- Add more negative number tests
- Clean up implementation

## Verification

Before claiming TDD was followed, verify:

- [ ] Every piece of production code has a corresponding test
- [ ] Tests were written BEFORE the code they test
- [ ] Each test was seen to fail before code was written
- [ ] Minimal code was written to pass each test
- [ ] All tests pass after implementation

If any checkbox is unchecked, TDD was not properly followed.
