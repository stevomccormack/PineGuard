# Snapshot provenance

Captured on 2026-09-26 from the local PineGuard checkout and project mirror. These are byte-exact reference snapshots; source files were not modified. SHA256 is calculated over each copied file and matches the source at capture.

| Snapshot | Source | SHA256 |
|---|---|---|
| `baseline-v1.6.txt` | `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\ai\plans\technical-readiness-to-1.0.md` | `72D86BC243DFD359F26496A6BA40761017D6E9E16A3DAB83875B5F445F8BA389` |
| `readiness-rubric-2026-09-25.txt` | `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\reports\technical-readiness-rubric-2026-09-25.md` | `FF2CE1C5F0CA71294CF21A1C3D5C6E7A3BF118DB1FE6C1FDD290B5950A17E5D5` |
| `readme-verification-2026-09-25.txt` | `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\reports\readme-verification-2026-09-25.md` | `8354A14532C22FC15D84A30C79A411E4AF93A6C2F4363DE2869D2E937084A297` |
| `README-2026-09-25.txt` | `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\README.md` | `38C4ABD0CCF3E9F9799B050FC23B8CA618424C06ECB4B320282F5A52F7AD75C0` |
| `original-engineering-handoff.txt` | `C:\Users\stevo\.codex\.chatgpt-projects\g-p-692fade03fc08191ad5558b875e2cc7f\PineGuard-Engineering-Handoff.md` | `99427BC9E98B51540F4E938B3147993CE7A2A400B80DD225E4D14AF62C31A4DF` |
| `baseline-test-evidence.json` | `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\artifacts\testing\20260925-full-suite-postrepair-terra\evidence.json` | `C253E3D82CCFCA103F6FCC7E96A317D969E728F7FC500DC0BD3D8DB7C68E70A0` |

## Historical working-tree status and limits

The repository documents and plan were captured from the local working tree, not reconstructed from a clean commit. At the source audit, `README.md` was modified; the v1.6 plan and the two dated reports were untracked local files. The evidence file is a generated artifact under the repository's ignored `artifacts/testing` tree. The handoff came from the local ChatGPT project mirror. Treat these as historical review inputs, not as claims that those documents were committed or that the handoff is authoritative. The original v1.6 file and unrelated pre-existing work remain untouched.

The evidence JSON records a 2026-09-25 full-suite run at commit `2aeac39b88effef5518741a9cf9b505a04f926f4`: 30 TRX files, 37,749 reported passing executions (18,862 net8.0 and 18,887 net10.0), and zero reported failures or skips. This packet does not include the raw TRX files and this snapshot task ran no tests and collected no coverage. These reported counts are evidence-summary values only; they do not prove coverage completeness or current behavior.
