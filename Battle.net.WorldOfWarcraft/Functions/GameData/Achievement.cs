namespace Battle.net.WorldOfWarcraft.Functions.GameData;

public class AchievementFunction(IApiRequestExecutor api) : BaseFunction(api), IAchievementFunction
{
    public AchievementModel GetAchievement(int achievementId)
    {
        return ApiRequest<AchievementModel>($"/achievement/${achievementId}");
    }

    public AchievementCategoriesModel GetAchievementCategories()
    {
        return ApiRequest<AchievementCategoriesModel>($"/achievement-category/index");
    }

    public AchievementCategoryModel GetAchievementCategory(int achievementCategoryId)
    {
        return ApiRequest<AchievementCategoryModel>($"/achievement-category/{achievementCategoryId}");
    }

    public AchievementMediaModel GetAchievementMedia(int achievementId)
    {
        return ApiRequest<AchievementMediaModel>($"/media/achievement/{achievementId}");
    }

    public AchievementIndexModel GetAchievementsIndex()
    {
        return ApiRequest<AchievementIndexModel>($"/achievement/index");
    }
}

public interface IAchievementFunction
{
    AchievementCategoriesModel GetAchievementCategories();
    AchievementCategoryModel GetAchievementCategory(int achievementCategoryId);
    AchievementIndexModel GetAchievementsIndex();
    AchievementModel GetAchievement(int achievementId);
    AchievementMediaModel GetAchievementMedia(int achievementId);
}