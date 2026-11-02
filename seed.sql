DELETE FROM "Coaches";
DELETE FROM "Plans";

INSERT INTO "Coaches" ("Name","IsActive",gender) VALUES
  ('محمود', true, 1), ('أبو فراس', true, 1), ('بشار', true, 1),
  ('ختام', true, 2), ('دعاء', true, 2);

INSERT INTO "Plans" ("Name","DurationInDays","Price","WithCoach","ProgramPrice") VALUES
  ('اشتراك يومي', 1, 100, false, 0),
  ('اشتراك شهري', 30, 1000, false, 800),
  ('اشتراك شهري مع مدرب', 30, 1200, true, 800);