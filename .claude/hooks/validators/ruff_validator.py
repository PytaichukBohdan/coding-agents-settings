# /// script
# requires-python = ">=3.11"
# dependencies = []
# ///
"""Ruff linting validator for builder agent PostToolUse hook.

Runs `ruff check` on Python files after Write/Edit operations.
Always exits 0 to avoid blocking Claude Code.
"""

import json
import os
import subprocess
import sys
from datetime import datetime
from pathlib import Path


def get_log_dir() -> Path:
    project_dir = os.environ.get("CLAUDE_PROJECT_DIR", ".")
    log_dir = Path(project_dir) / "logs"
    log_dir.mkdir(parents=True, exist_ok=True)
    return log_dir


def log(message: str) -> None:
    log_dir = get_log_dir()
    log_file = log_dir / "ruff_validator.log"
    timestamp = datetime.now().isoformat()
    with open(log_file, "a") as f:
        f.write(f"[{timestamp}] {message}\n")


def extract_file_path(hook_data: dict) -> str | None:
    """Extract file path from PostToolUse hook data."""
    try:
        tool_name = hook_data.get("tool_name", "")
        tool_input = hook_data.get("tool_input", {})

        if tool_name == "Write":
            return tool_input.get("file_path")
        elif tool_name == "Edit":
            return tool_input.get("file_path")
        return None
    except (KeyError, TypeError):
        return None


def main() -> None:
    try:
        raw = sys.stdin.read()
        if not raw.strip():
            sys.exit(0)

        hook_data = json.loads(raw)
        file_path = extract_file_path(hook_data)

        if not file_path or not file_path.endswith(".py"):
            sys.exit(0)

        if not Path(file_path).exists():
            log(f"File not found: {file_path}")
            sys.exit(0)

        log(f"Running ruff check on: {file_path}")

        result = subprocess.run(
            ["ruff", "check", file_path],
            capture_output=True,
            text=True,
            timeout=30,
        )

        if result.returncode == 0:
            log(f"ruff check passed: {file_path}")
        else:
            log(f"ruff check issues in {file_path}:\n{result.stdout}\n{result.stderr}")
            # Print to stderr so Claude Code can see it
            print(result.stdout, file=sys.stderr)

    except json.JSONDecodeError:
        log("Failed to parse hook JSON from stdin")
    except FileNotFoundError:
        log("ruff not found in PATH - skipping check")
    except subprocess.TimeoutExpired:
        log("ruff check timed out")
    except Exception as e:
        log(f"Unexpected error: {e}")

    # Always exit 0
    sys.exit(0)


if __name__ == "__main__":
    main()
