const fs = require("fs");
const path = require("path");

const srcPath = path.join(__dirname, "..", "tmp-ly-3479.js");
const outDir = path.join(__dirname, "..", "content", "dsa-sheet");
const src = fs.readFileSync(srcPath, "utf8");

const GROUP_ENUM = {
  COMPLEXITY: "Time and Space Complexity / Online Judge",
  DSA_FUNDAMENTALS: "Dsa Fundamentals",
  PROGRAMMING_FUNDAMENTALS: "Programming Fundamentals",
  BASIC_ARRAY_AND_STRING: "Array and String",
  BASIC_MATHS: "Basic Maths Level 1",
  RECURSION_BASICS: "Recursion Basics",
  SORTING: "Sorting",
  TWO_POINTERS: "2 Pointers",
  PREFIX_SUM: "Prefix Sum",
  MATRIX: "Matrix",
  HASHING: "Hashing",
  SLIDING_WINDOW: "Sliding Window",
  LINKED_LIST: "Linked List",
  STACK: "Stack",
  QUEUE: "Queue",
  BINARY_SEARCH: "Binary Search",
  MATH_2: "Math Level 2",
  BIT_MANIPULATION: "Bit Manipulation",
  RECURSION_BACKTRACKING: "Recursion & Backtracking",
  TREE_BST: "Tree + BST",
  HEAP_PRIORITY_QUEUE: "Heap (Priority Queue)",
  TRIES: "Tries",
  GREEDY: "Greedy",
  DYNAMIC_PROGRAMMING: "Dynamic Programming",
  DYNAMIC_PROGRAMMING_1: "Dynamic Programming Level 1",
  DYNAMIC_PROGRAMMING_2: "Dynamic Programming Level 2",
  BST: "Binary Search Tree",
  BINARY_TREE: "Binary Tree",
  GRAPHS: "Graphs",
  STRING_MATCHING_ALGOS: "String Matching Algos",
  COMBINATORICS_GEOMETRY: "Combinatorics & Geometry",
  GAME_THEORY: "Game Theory",
  ADVANCED_ALGO_SEGMENT_FENWICK: "Advance algorithm",
  ANY: "Any",
};

function extractObjectLiteral(src, constName) {
  const marker = `const ${constName} =`;
  const start = src.indexOf(marker);
  if (start < 0) throw new Error(`Missing ${constName}`);
  let i = start + marker.length;
  while (src[i] && /\s/.test(src[i])) i++;
  if (src[i] !== "[") throw new Error(`${constName} is not an array`);
  let depth = 0;
  let inStr = false;
  let strCh = "";
  let esc = false;
  const begin = i;
  for (; i < src.length; i++) {
    const c = src[i];
    if (inStr) {
      if (esc) {
        esc = false;
        continue;
      }
      if (c === "\\") {
        esc = true;
        continue;
      }
      if (c === strCh) inStr = false;
      continue;
    }
    if (c === '"' || c === "'" || c === "`") {
      inStr = true;
      strCh = c;
      continue;
    }
    if (c === "[") depth++;
    else if (c === "]") {
      depth--;
      if (depth === 0) return src.slice(begin, i + 1);
    }
  }
  throw new Error(`Unclosed array for ${constName}`);
}

function parseGroups(literal) {
  const js = literal
    .replace(/Problems_types\/\* PROBLEM_GROUPS \*\/\.L1\.(\w+)/g, (_, k) =>
      JSON.stringify(GROUP_ENUM[k] || k)
    )
    .replace(/,\s*([\]}])/g, "$1");
  // eslint-disable-next-line no-new-func
  return Function(`"use strict"; return (${js});`)();
}

function jsString(raw) {
  // Evaluate a JS double-quoted string body (\xHH, \uHHHH, \n, \", etc.)
  return Function(`"use strict"; return "${raw}";`)();
}

function extractProblems(src) {
  const problems = new Map();
  const re =
    /\{\s*id:\s*"(\d+)"\s*,\s*title:\s*"((?:\\.|[^"\\])*)"\s*,\s*difficulty:\s*Problems_types\/\* PROBLEM_DIFFICULTY \*\/\.\$T\.(\w+)([\s\S]*?)\n\s*\}/g;
  let m;
  while ((m = re.exec(src))) {
    const id = m[1];
    const title = jsString(m[2]);
    const difficulty = m[3];
    const body = m[4];
    const linkM = body.match(/problemLink:\s*"((?:\\.|[^"\\])*)"/);
    const articleM = body.match(/articleLink:\s*"((?:\\.|[^"\\])*)"/);
    const platformM = body.match(/platform:\s*"((?:\\.|[^"\\])*)"/);
    const videoM = body.match(/videoLink:\s*"((?:\\.|[^"\\])*)"/);
    const tagsM = body.match(/tags:\s*\[([\s\S]*?)\]/);
    let tags = [];
    if (tagsM) {
      tags = [...tagsM[1].matchAll(/"((?:\\.|[^"\\])*)"/g)].map((x) =>
        jsString(x[1])
      );
    }
    problems.set(id, {
      id,
      title,
      difficulty,
      tags,
      problemLink: linkM ? jsString(linkM[1]).trim() : null,
      articleLink: articleM ? jsString(articleM[1]).trim() : null,
      platform: platformM ? jsString(platformM[1]) : null,
      videoLink: videoM ? jsString(videoM[1]) : null,
    });
  }
  return [...problems.values()];
}

const groupsLiteral = extractObjectLiteral(src, "PROBLEM_GROUP_LIST_PRACTICE_MODE");
const groups = parseGroups(groupsLiteral);
const problems = extractProblems(src);
const byId = Object.fromEntries(problems.map((p) => [p.id, p]));

function enrich(groups) {
  return groups.map((g) => ({
    id: g.id,
    title: g.title,
    description: g.description || "",
    subgroups: (g.subgroups || []).map((sg) => ({
      id: sg.id,
      title: sg.title,
      description: sg.description || "",
      problems: (sg.problems || []).map((pid) => {
        const p = byId[pid];
        if (!p) return { id: pid, missing: true };
        return p;
      }),
    })),
  }));
}

const sheet = {
  source: "https://learnyard.com/practice/dsa",
  note: "Topic/problem index extracted from LearnYard DSA Sheet (LeetCode/public links). Lesson bodies are first-party InterviewTutor content — not scraped articles.",
  extractedAt: new Date().toISOString(),
  groupCount: groups.length,
  problemCount: problems.length,
  groups: enrich(groups),
};

fs.mkdirSync(outDir, { recursive: true });
fs.writeFileSync(path.join(outDir, "practice-sheet.json"), JSON.stringify(sheet, null, 2));

// Compact markdown index for humans
const lines = [
  "# DSA Practice Sheet",
  "",
  `Source structure from [LearnYard DSA Sheet](${sheet.source}).`,
  "",
  `**${sheet.groupCount} topic groups · ${sheet.problemCount} problems/theory items**`,
  "",
];
for (const g of sheet.groups) {
  lines.push(`## ${g.title}`, "");
  for (const sg of g.subgroups) {
    lines.push(`### ${sg.title}`, "");
    for (const p of sg.problems) {
      if (p.missing) {
        lines.push(`- [ ] missing id ${p.id}`);
        continue;
      }
      const link = p.problemLink && p.problemLink !== "NA" ? p.problemLink : p.articleLink;
      const label = link ? `[${p.title}](${link})` : p.title;
      lines.push(`- [ ] **${p.difficulty}** ${label}`);
    }
    lines.push("");
  }
}
fs.writeFileSync(path.join(outDir, "README.md"), lines.join("\n"));

// Summary of subgroup titles for curriculum gap analysis
const subgroupTitles = [];
for (const g of sheet.groups) {
  for (const sg of g.subgroups) subgroupTitles.push(`${g.title} > ${sg.title}`);
}
fs.writeFileSync(
  path.join(outDir, "topics.txt"),
  subgroupTitles.join("\n") + "\n"
);

console.log(
  JSON.stringify(
    {
      groups: sheet.groupCount,
      problems: sheet.problemCount,
      out: outDir,
      sampleGroups: sheet.groups.slice(0, 3).map((g) => g.title),
    },
    null,
    2
  )
);
