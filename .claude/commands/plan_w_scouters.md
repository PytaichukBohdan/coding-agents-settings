---
description: Use when you need to plan implementation of a feature, fix, or refactor with parallel scout agents for codebase exploration
argument-hint: [user prompt]
---

# Quick Plan

Create a detailed implementation plan based on the user's requirements provided through the `USER_PROMPT` variable. Analyze the request, think through the implementation approach, and save a comprehensive specification document to `PLAN_OUTPUT_DIRECTORY/<name-of-plan>.md` that can be used as a blueprint for actual development work. Follow the `Instructions` and work through the `Workflow` to create the plan.

## Variables

USER_PROMPT: $1
PLAN_OUTPUT_DIRECTORY: `specs/`
TOTAL_BASE_SCOUT_SUBAGENTS: 3
TOTAL_FAST_SCOUT_SUBAGENTS: 5

## Instructions

- IMPORTANT: If no `USER_PROMPT` is provided, STOP immediately and ask.

  **No exceptions:**
  - Don't infer the prompt from context
  - Don't use previous conversation as the prompt
  - Don't proceed with "I'll ask later if needed"
  - STOP means STOP
- Carefully analyze the user's requirements provided in the USER_PROMPT variable
- Determine the task type (chore|feature|refactor|fix|enhancement) and complexity (simple|medium|complex)
- Think deeply (ultrathink) about the best approach to implement the requested functionality or solve the problem
- Explore the codebase to understand existing patterns and architecture
- Follow the Plan Format below to create a comprehensive implementation plan
- Include all required sections and conditional sections based on task type and complexity
- Generate a descriptive, kebab-case filename based on the main topic of the plan
- Save the complete implementation plan to `PLAN_OUTPUT_DIRECTORY/<descriptive-name>.md`
- Ensure the plan is detailed enough that another developer could follow it to implement the solution
- Include code examples or pseudo-code where appropriate to clarify complex concepts
- Consider edge cases, error handling, and scalability concerns
- Use `AskUserQuestion` tool ONLY when there's genuine ambiguity or a critical decision point - avoid asking questions just to appear thorough. If the requirements are clear from the USER_PROMPT and codebase exploration, proceed directly with planning.

## The Iron Law

```
NO IMPLEMENTATION WITHOUT A COMPLETE PLAN FIRST
```

A plan is complete when another developer could implement it without asking questions.

Incomplete plan? Don't proceed. Add details until complete.

**No exceptions:**
- Don't skip codebase exploration "because it seems simple"
- Don't assume you know the architecture
- Don't proceed with "I'll figure it out as I go"
- Incomplete means INCOMPLETE - add more detail

## Red Flags - STOP Planning

If any of these thoughts occur to you, STOP and reconsider:

- About to write code without understanding requirements
- Skipping codebase exploration "because it's simple"
- Assuming you know the architecture
- "I'll just start and figure it out"
- "The user will figure out the details"
- Creating a plan without validation commands
- Skipping scout agents "to save time"

**If any of these apply: STOP. Re-read the workflow.**

## The Planning Gate

BEFORE saving the plan:
1. **Read requirements again** - did you address everything?
2. **Check scout results** - did you consolidate all findings?
3. **Review validation commands** - are they specific and runnable?
4. **Verify completeness** - could another developer implement this without questions?

If ANY answer is "no": continue planning, don't save yet.

## Common Rationalizations

| Excuse | Reality |
|--------|---------|
| "This is a simple task" | Simple tasks have hidden complexity. Plan thoroughly. |
| "I already know this codebase" | Every session starts fresh. Use the scouts. |
| "The user is in a hurry" | Rushed plans create more delays. Take time. |
| "I'll ask questions later" | Ask now. Blocked implementation wastes more time. |
| "Scout agents are overkill" | Scouts find things you'd miss. Use them. |

## Announcement (MANDATORY)

Before starting work, announce:

"I'm using /plan_w_scouters to create an implementation plan for [specific purpose]. I will deploy scout agents and follow the workflow exactly."

This creates commitment. Skipping this step = likely to skip other steps.

## Workflow

1. Analyze Requirements - THINK HARD and parse the USER_PROMPT to understand the core problem and desired outcome
2. Explore Codebase - Understand existing patterns, architecture, and relevant files. Deploy a total of `TOTAL_BASE_SCOUT_SUBAGENTS + TOTAL_FAST_SCOUT_SUBAGENTS` subagents in PARALLEL to scout the codebase for files needed to complete the task.
   1. Run `TOTAL_BASE_SCOUT_SUBAGENTS` @agent-scout-report-suggest agents in parallel to scout the codebase for files needed to complete the task. Give each different direction, divide and conquer.
   2. Run `TOTAL_FAST_SCOUT_SUBAGENTS` @agent-scout-report-suggest-fast agents in parallel to scout the codebase for files needed to complete the task. Give each different direction, divide and conquer.
   3. Gather the results from the subagents and consolidate them into a single list of files needed to complete the task. 
   4. Validate the files - consolidate the results from the subagents and then MANUALLY validate, double check, and look for any missing files needed to complete the task.
4. Design Solution - Develop technical approach including architecture decisions and implementation strategy
5. Document Plan - Structure a comprehensive markdown document with problem statement, implementation steps, and testing approach
6. Generate Filename - Create a descriptive kebab-case filename based on the plan's main topic
7. Save & Report - Follow the `Report` section to write the plan to `PLAN_OUTPUT_DIRECTORY/<filename>.md` and provide a summary of key components

## Plan Format

Follow this format when creating implementation plans:

```md
# Plan: <task name>

## Task Description
<describe the task in detail based on the prompt>

## Objective
<clearly state what will be accomplished when this plan is complete>

<if task_type is feature or complexity is medium/complex, include these sections:>
## Problem Statement
<clearly define the specific problem or opportunity this task addresses>

## Solution Approach
<describe the proposed solution approach and how it addresses the objective>
</if>

## Relevant Files
Use these files to complete the task:

<list files relevant to the task with bullet points explaining why. Include new files to be created under an h3 'New Files' section if needed>

<if complexity is medium/complex, include this section:>
## Implementation Phases
IMPORTANT: Each phase should be a checkbox that will be checked off during implementation. Include Status and Comments fields for tracking progress.

- [ ] **Phase 1: Foundation** - <describe any foundational work needed>
  - Status:
  - Comments:

- [ ] **Phase 2: Core Implementation** - <describe the main implementation work>
  - Status:
  - Comments:

- [ ] **Phase 3: Integration & Polish** - <describe integration, testing, and final touches>
  - Status:
  - Comments:
</if>

## Step by Step Tasks
IMPORTANT: Execute every step in order, top to bottom. Each task should be a checkbox that will be checked off during implementation. Include Status and Comments fields for tracking progress.

<list step by step tasks as checkboxes with h3 headers for grouping. Format each task as:>
- [ ] **Task description** - <detailed explanation>
  - Status:
  - Comments:

<Use h3 headers to organize tasks by category. Start with foundational changes then specific implementation. Last step should validate the work.>

### 1. <Category Name>
- [ ] **<Task Name>** - <specific action>
  - Status:
  - Comments:

### 2. <Category Name>
- [ ] **<Task Name>** - <specific action>
  - Status:
  - Comments:

<continue with additional tasks as needed>

<if task_type is feature or complexity is medium/complex, include this section:>
## Testing Strategy
<describe testing approach, including unit tests and edge cases as applicable>
</if>

## Acceptance Criteria
<list specific, measurable criteria that must be met for the task to be considered complete>

## Validation Commands
Execute these commands to validate the task is complete:

<list specific commands to validate the work. Be precise about what to run>
- Example: `uv run python -m py_compile src/*.py` - Test to ensure the code compiles

## Notes
<optional additional context, considerations, or dependencies. If new libraries are needed, specify using `uv add`>
```

## Report

After creating and saving the implementation plan, provide a concise report with the following format:

```
✅ Implementation Plan Created

File: PLAN_OUTPUT_DIRECTORY/<filename>.md
Topic: <brief description of what the plan covers>
Key Components:
- <main component 1>
- <main component 2>
- <main component 3>
```
