﻿namespace AgentSimulation.Utilities {
    public record class Config(int Replications, int WorkersA, int WorkersB, int WorkersC, int Workplaces);
    public record class Config1() : Config(500, 6, 5, 38, 58);
    public record class Config2() : Config(500, 9, 5, 25, 58);
    public record class Config3() : Config(500, 6, 7, 38, 45);
    public record class Config4() : Config(500, 9, 7, 25, 45);
}
