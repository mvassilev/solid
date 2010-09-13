// OpenF
// Authors: 2004 A.Penev
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

// 01.01.2003 Initial version. A.Penev (alexander_penev@yahoo.com)

using System;
using System.Collections.Generic;

namespace SolidOpt.Services
{
	public interface IServiceProvider : IService
	{
		Service GetService<Service>() where Service : class;
		List<Service> GetServices<Service>() where Service : class;
		IService GetService(Type serviceType);
		IService[] GetServices(Type serviceType);
	}
	
}

