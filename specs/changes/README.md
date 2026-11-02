# Active Changes Directory (`specs/changes/`)

Store active feature proposals here while under development.
Each active change must have its own folder:
```
specs/changes/<change-id>/
├── proposal.md   # What and why (motivation and high-level requirements)
├── design.md     # Architecture Decision Record (ADR) & technical design (optional for simple changes)
└── tasks.md      # Atomic, test-first task list
```

Once shipped and verified, move the completed folder to `specs/changes/archive/YYYY-MM-DD-<change-id>/`.
