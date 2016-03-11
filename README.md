#CandidateManager

This is a simple application that helps to dispense practical tests to candidates trying to apply to a company's open position. Is possible to define a list of candidates and a list of exercises (represented by the file that will be downloaded at the test start). Given a candidate and an exercise is then possible to define a practical test session.

Each session can be configured with:
- Its period of validity, when is possible to start the test.
- The time the candidate is allowed to spend in order to finish the test.

An email system is provided to notify:
- When a test is published, this is notified to the candidate.
- When a test has been started.
- When a test has been submitted.

All the data is stored in a code-first EF database.
