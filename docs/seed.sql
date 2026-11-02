mkdir -p docs
cat > docs/seed.sql <<'EOF'
INSERT INTO "Coaches" ("Name","IsActive","Gender") VALUES
  ('ãÍãæÏ', true, 1), ('ÃÈæ ÝÑÇÓ', true, 1), ('ÈÔÇÑ', true, 1),
  ('ÎÊÇã', true, 2), ('ÏÚÇÁ', true, 2);
INSERT INTO "Plans" ("Name","DurationInDays","Price","WithCoach","ProgramPrice") VALUES
  ('ÇÔÊÑÇß íæãí', 1, 100, false, 0),
  ('ÇÔÊÑÇß ÔåÑí', 30, 1000, false, 800),
  ('ÇÔÊÑÇß ÔåÑí ãÚ ãÏÑÈ', 30, 1200, true, 800);
EOF