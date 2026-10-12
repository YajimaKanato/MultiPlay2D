# Codex instructions

## Language

- GitHub pull request reviews, replies, explanations, and generated diagrams must be written in Japanese.
- Keep source-code identifiers, API names, type names, and method signatures in their original form.

## GitHub mention: クラス可視化

When a pull request comment mentions Codex with `@codex クラス可視化`, treat it as a request to explain the architecture introduced or changed by that pull request. Do not perform an ordinary defect review unless the comment also explicitly requests one.

Analyze the pull request diff first, then read the minimum relevant surrounding code needed to understand inheritance, interfaces, ownership, dependencies, construction, calls, events, and data flow. Focus on classes and behavior added or materially changed by the pull request. Include unchanged classes only when they are necessary to explain how the changed code is actually used.

Post the result as one top-level comment in the pull request Conversation. The response must be self-contained Markdown that GitHub can render directly. Do not create or commit diagram files, images, documentation files, or source-code changes for this request. Put Mermaid diagrams directly in fenced `mermaid` blocks inside the Conversation comment.

The Japanese Conversation comment must contain all of the following:

1. **概要**
   - Briefly explain what the changed classes are intended to accomplish.
   - State any important assumptions or uncertainty caused by missing implementations, runtime wiring, serialized Unity references, or code outside the pull request.

2. **クラス図**
   - Use a GitHub-renderable Mermaid `classDiagram` fenced block.
   - Show each relevant class, interface, enum, and important data type.
   - Show inheritance, interface implementation, composition, aggregation, ownership, and meaningful dependencies with labeled relationships where useful.
   - For every relevant class, include its important members: fields, properties, constructors, public/protected methods, and private methods that are essential to the described flow.
   - Use UML visibility markers where known: `+` public, `#` protected, `-` private, `~` internal/package.
   - Do not invent members or relationships. If a relationship is inferred rather than explicit, label it as an inference in the surrounding text.

3. **想定される使い方・処理フロー**
   - Use one or more GitHub-renderable Mermaid `sequenceDiagram` blocks to show the main expected runtime scenario from entry point to result.
   - Show who creates or owns each object, which method calls which method, important arguments or returned values, state changes, callbacks/events, asynchronous boundaries, and error or alternative paths when relevant.
   - If the behavior is state-driven, branch-heavy, or clearer as control flow, add a Mermaid `flowchart` as well.
   - Base the scenario on actual call sites and wiring in the pull request or repository. Clearly mark any proposed or inferred invocation that has no concrete call site.

4. **実際の利用例**
   - Describe the recommended call order as a short numbered list.
   - Include a concise code example only when it materially clarifies initialization or invocation. Use existing APIs exactly as implemented.

5. **注意点**
   - List lifecycle constraints, required initialization order, nullable or serialized dependencies, ownership/disposal concerns, networking or concurrency assumptions, and gaps where the intended usage cannot be proven from the code.

Keep diagrams readable. If one diagram becomes crowded, split it by responsibility or scenario. Prefer precise, code-grounded content over exhaustive inclusion of unrelated repository classes.

