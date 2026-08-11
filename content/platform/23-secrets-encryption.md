---
id: platform-secrets-encryption
title: Secrets, Encryption, and Transport Security
track: platform
module: "03 Security"
order: 23
languages: [java, csharp]
summary: TLS, secret storage, hashing passwords, encryption at rest — practical interview depth.
---

## Why this matters

Leaked secrets and weak password storage still cause real breaches. Seniors speak precisely about crypto use-cases.

## Definitions

- **Secret:** Sensitive credential or key (API keys, signing keys, DB passwords) that must be stored, injected, and rotated safely.
- **TLS:** Transport encryption plus server authentication for data in motion (HTTPS everywhere).
- **Encryption at rest:** Protecting stored data on disks, DB columns, and backups with keys managed outside the data.
- **Password hashing:** Slow one-way KDF (Argon2id/bcrypt/scrypt) with a unique salt—not reversible encryption.
- **Salt:** Per-password random value that defeats precomputed rainbow-table attacks.
- **KMS / Vault:** Systems that store, issue, and rotate keys/secrets with audit and access control.
- **mTLS:** Mutual TLS—both client and server present certificates for service-to-service trust.


## Passwords

```text
store argon2id(password, salt, params)
never store reversible password encryption
never use plain MD5/SHA1 for passwords
```

## Secrets management

- Inject via env/secret store at runtime  
- Rotate JWT signing keys with `kid`  
- Separate prod/dev credentials  
- Scan git history; prefer short-lived credentials

## Interview Q&A

- **Q:** Hash vs encrypt?
  **A:** Hash for passwords (verify by re-hash); encrypt when you must decrypt (e.g. some tokens at rest).
- **Q:** Where do keys live?
  **A:** KMS/HSM/cloud secret manager — not in source control.
- **Q:** Field-level encryption?
  **A:** For sensitive columns; trade-offs on search/indexing.

## Pitfalls

- Rolling your own crypto  
- Logging Authorization headers  
- Long-lived static access keys in mobile apps

## 60-second answer

“TLS in transit, strong password hashing with salt, secrets in a vault with rotation, and encryption at rest for sensitive fields. I don’t invent crypto protocols.”

## Further study

- [OWASP Password Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html) — Argon2/bcrypt parameters and anti-patterns.
- [OWASP Cryptographic Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cryptographic_Storage_Cheat_Sheet.html) — at-rest encryption and key management basics.
- [MDN: Transport Layer Security (TLS)](https://developer.mozilla.org/en-US/docs/Web/Security/Transport_Layer_Security) — what TLS protects on the wire.
- [Microsoft Learn: Azure Key Vault](https://learn.microsoft.com/en-us/azure/key-vault/general/basic-concepts) — secrets/keys/certs management model.

## Practice prompts

1. Design JWT key rotation with overlap  
2. Threat-model secrets in a CI pipeline  
3. Choose argon2 parameters at a high level
