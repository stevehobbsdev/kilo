
namespace Kilo.Configuration.Providers
{
	public interface ILoadable
	{
		bool CanLoad { get; }

		void Load();
	}
}
