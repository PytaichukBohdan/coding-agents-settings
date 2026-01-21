# AGENTS

<skills_system priority="1">

## Available Skills

<!-- SKILLS_TABLE_START -->
<usage>
When users ask you to perform tasks, check if any of the available skills below can help complete the task more effectively. Skills provide specialized capabilities and domain knowledge.

How to use skills:
- Invoke: Bash("openskills read <skill-name>")
- The skill content will load with detailed instructions on how to complete the task
- Base directory provided in output for resolving bundled resources (references/, scripts/, assets/)

Usage notes:
- Only use skills listed in <available_skills> below
- Do not invoke a skill that is already loaded in your context
- Each skill invocation is stateless
</usage>

<available_skills>

<skill>
<name>fork-terminal</name>
<description>Fork a terminal session to a new terminal window. Use this when the user requests 'fork terminal' or 'create a new terminal' or 'new terminal: <command>' or 'fork session: <command>'.</description>
<location>.claude/skills/fork-terminal/</location>
</skill>

<skill>
<name>meta-skill</name>
<description>Creates new Agent Skills for AI Agents following best practices and documentation. Use when the user wants prompts 'create a new skill ...' or 'use your meta skill to ...'.</description>
<location>.claude/skills/meta-skill/</location>
</skill>

<skill>
<name>systematic-debugging</name>
<description>Enforces a methodical approach to debugging that prevents trial-and-error fixes. Use when investigating bugs, errors, or unexpected behavior in code.</description>
<location>.claude/skills/systematic-debugging/</location>
</skill>

<skill>
<name>test-driven-development</name>
<description>Enforces the test-driven development methodology when implementing features or bugfixes. Use when writing any new code, fixing bugs, or implementing features that need tests.</description>
<location>.claude/skills/test-driven-development/</location>
</skill>

<skill>
<name>verification-before-completion</name>
<description>Enforces evidence-based verification before claiming any task is complete. Use when finishing any implementation, fix, or task that requires proof of completion.</description>
<location>.claude/skills/verification-before-completion/</location>
</skill>

</available_skills>
<!-- SKILLS_TABLE_END -->

</skills_system>
